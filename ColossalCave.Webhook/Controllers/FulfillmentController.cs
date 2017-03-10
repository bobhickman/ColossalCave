using System;
using System.Collections.Generic;
using System.Linq;
using ColossalCave.Engine;
using ColossalCave.Engine.Interfaces;
using ColossalCave.Engine.Utilities;
using ColossalCave.Webhook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ColossalCave.Webhook.Controllers
{
    [Route("api/[controller]")]
    public class FulfillmentController : Controller
    {
        private readonly ILogger _log;

        private readonly IActionHandler _handler;
        private readonly AdventureContext _advContext;

        public FulfillmentController(ILogger<FulfillmentController> log, 
            IActionHandler handler, AdventureContext context)
        {
            _log = log;
            _handler = handler;
            _advContext = context;
        }

        // POST api/Fulfillment
        [HttpPost]
        public ApiAiFulfillmentResponse Post([FromBody]ApiAiFulfillmentRequest request)
        {
            try
            {
                _log.LogInformation("Received fulfillment request.");
                _log.LogInformation("Intent is " + request.Result.Metadata.IntentName);

                _advContext.ContextId = request.SessionId;
                _advContext.CurrentLocationId = 1;
                _advContext.IntentName = request.Result.Metadata.IntentName;
                _advContext.Parameters = request.Result.Parameters;

                var context = _advContext;

                if (request.Result.Contexts != null && request.Result.Contexts.Length > 0)
                {
                    var requestAdvContext = request.Result.Contexts.FirstOrDefault(c => c.Name.EqualsNoCase("AdventureContext"));
                    if (requestAdvContext != null && requestAdvContext.Parameters != null)
                    {
                        if (requestAdvContext.Parameters.ContainsKey("CurrentLocationId"))
                            context.CurrentLocationId = int.Parse(requestAdvContext.Parameters["CurrentLocationId"]);
                        if (requestAdvContext.Parameters.ContainsKey("Flags"))
                        {
                            var flagStr = requestAdvContext.Parameters["Flags"];
                            var bytes = Convert.FromBase64String(flagStr);
                            context.Flags = (Flags)BitConverter.ToInt32(bytes, 0);
                        }
                    }
                };

                _handler.Handle();

                _log.LogInformation("Creating response...");
                _log.LogInformation(context.SpeechResponse);
                var response = new ApiAiFulfillmentResponse
                {
                    Speech = context.SpeechResponse,
                    DisplayText = context.TextResponse,
                    Source = "apiWebhook",
                    //Data = "",
                    ContextOut = new[] 
                    {
                        new ApiAiContext
                        {
                            Name = "AdventureContext",
                            Lifespan = 100,
                            Parameters = new Dictionary<string, string>
                            {
                                { "CurrentLocationId", context.CurrentLocationId.ToString() },
                                { "Flags", Convert.ToBase64String(BitConverter.GetBytes((Int32)context.Flags)) }
                            }
                        }
                    },
                    //FollowupEvent = ""
                };
                _log.LogInformation("Sending response...");
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return new ApiAiFulfillmentResponse
                {
                    Speech = ex.Message,
                    DisplayText = ex.Message
                };
            }
        }
    }
}

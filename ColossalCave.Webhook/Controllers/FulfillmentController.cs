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
        //private readonly IItemProvider _itemProvider;
        //private readonly ILocationProvider _locationProvider;
        private readonly AdventureContext _advContext;

        public FulfillmentController(ILogger<FulfillmentController> log, 
            IActionHandler handler, 
            //ILocationProvider locationProvider, IItemProvider itemProvider, 
            AdventureContext context)
        {
            _log = log;
            _handler = handler;
            //_itemProvider = itemProvider;
            //_locationProvider = locationProvider;
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

                ApiContextToAdvContext(request);

                _handler.Handle();

                _log.LogInformation("Creating response...");
                _log.LogInformation(_advContext.SpeechResponse);
                var response = new ApiAiFulfillmentResponse
                {
                    Speech = _advContext.SpeechResponse,
                    DisplayText = _advContext.TextResponse,
                    Source = "apiWebhook",
                    //Data = "",
                    ContextOut = new[]
                    {
                        AdvContextToApiContext()
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

        private ApiAiContext AdvContextToApiContext()
        {
            return new ApiAiContext
            {
                Name = "AdventureContext",
                Lifespan = 100,
                Parameters = new Dictionary<string, string>
                {
                    { "CurrentLocationId", _advContext.CurrentLocation.Id.ToString() },
                    { "Flags", Convert.ToBase64String(BitConverter.GetBytes((Int32)_advContext.Flags)) },
                    { "ItemLocations", _advContext.ItemLocationsToJson() },
                    { "ItemsMoveableStates", _advContext.StatesToJson() }
                }
            };
        }

        private void ApiContextToAdvContext(ApiAiFulfillmentRequest request)
        {
            _advContext.ContextId = request.SessionId;
            _advContext.SetCurrentLocation(1);
            _advContext.IntentName = request.Result.Metadata.IntentName;
            _advContext.Parameters = request.Result.Parameters;

            var requestAdvContext = request.Result.Contexts
                .FirstOrDefault(c => c.Name.EqualsNoCase("AdventureContext"));
            if (requestAdvContext != null && requestAdvContext.Parameters != null)
            {
                if (requestAdvContext.Parameters.ContainsKey("CurrentLocationId"))
                {
                    var locationId = int.Parse(requestAdvContext.Parameters["CurrentLocationId"]);
                    _advContext.SetCurrentLocation(locationId);
                }
                if (requestAdvContext.Parameters.ContainsKey("Flags"))
                {
                    var flagStr = requestAdvContext.Parameters["Flags"];
                    var bytes = Convert.FromBase64String(flagStr);
                    _advContext.Flags = (AdventureContextFlags)BitConverter.ToInt32(bytes, 0);
                }
                if (requestAdvContext.Parameters.ContainsKey("ItemLocations"))
                {
                    _advContext.ItemLocationsFromJson(requestAdvContext.Parameters["ItemLocations"]);
                }
                if(requestAdvContext.Parameters.ContainsKey("ItemsMoveableStates"))
                {
                    _advContext.StatesFromJson(requestAdvContext.Parameters["ItemsMoveableStates"]);
                }
            }
        }
    }
}

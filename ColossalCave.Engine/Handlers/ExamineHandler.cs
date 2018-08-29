using System;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.Handlers
{
    public class ExamineHandler : BaseHandler, IExamineHandler
    {
        public ExamineHandler(ILogger<ExamineHandler> log,
           IResponseBuilder responseBuilder,
           IMessageProvider messageProvider,
           ILocationProvider locationProvider,
           IAdventureContextHelper advHelper,
           IMapHelper mapHelper,
           AdventureContext context)
            : base(log, responseBuilder, messageProvider, locationProvider, advHelper, mapHelper)
        {
        }

        public override void Handle()
        {
            _log.LogInformation("Handling Examine");

            // Can't see in the dark.
            if (!_advHelper.IsCurrentLocationLight)
            {
                _responseBuilder.PrefixResponse(MsgMnemonic.MovePitchDark, 1);
                return;
            }

            var examineStr = _advHelper.GetParameterValue("visuals");
            var itemStr = _advHelper.GetParameterValue("items-moveable");
            if (itemStr == null)
                itemStr = _advHelper.GetParameterValue("treasures");
            if (itemStr == null)
                itemStr = _advHelper.GetParameterValue("items-fixed");
            if (itemStr == null)
                itemStr = _advHelper.GetParameterValue("mobs");
            if (Enum.TryParse(itemStr, true, out Items item))
            {
                // Item must be in the room or inventory
                if (_advHelper.IsItemAtCurrentLocation(item))
                {
                    var desc = _advHelper.GetItemExamination(item);
                    if (!string.IsNullOrEmpty(desc))
                        _responseBuilder.AddToResponse(desc);
                    else
                        _responseBuilder.AddToResponse("It's unremarkable.");
                }
                else
                {
                    _responseBuilder.AddToResponse(MsgMnemonic.ItemNotHere);
                }
            }
            else
            {
                _responseBuilder.PrefixResponse(MsgMnemonic.VocabDontUnderstand, 1);
            }
        }
    }
}

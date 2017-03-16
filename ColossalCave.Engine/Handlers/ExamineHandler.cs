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
            var itemMoveableStr = _advHelper.GetParameterValue("items-moveable");
            if (Enum.TryParse(itemMoveableStr, true, out ItemsMoveable itemMoveable))
            {
                // Item must be in the room or inventory
                if (_advHelper.IsItemAtCurrentLocation(itemMoveable))
                {
                    _responseBuilder.AddToResponse("Examining...");
                }
                else
                {
                    _responseBuilder.AddToResponse(MsgMnemonic.ItemNotHere);
                }
            }
            // TODO: ItemsFixed, Treasures, Mobs
            else
            {
                _responseBuilder.PrefixResponse(MsgMnemonic.VocabDontUnderstand, 1);
            }
        }
    }
}

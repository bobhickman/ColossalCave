using System;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.ActionHandlers
{
    public class ExamineHandler : BaseHandler, IExamineHandler
    {
        public ExamineHandler(ILogger<ExamineHandler> log,
            IResponseBuilder responseBuilder,
            IMessageProvider messageProvider,
            ILocationProvider locationProvider,
            IMapHelper mapHelper,
            AdventureContext context)
            : base(log, responseBuilder, messageProvider, locationProvider, mapHelper, context)
        {
        }

        public override void Handle()
        {
            _log.LogInformation("Handling Examine");

            // Can't see in the dark.
            if (!_advContext.IsCurrentLocationLight())
            {
                _responseBuilder.PrefixResponse(MsgMnemonic.MovePitchDark, 1);
                return;
            }

            var examineStr = _advContext.Parameters["visuals"];
            var itemMoveableStr = _advContext.Parameters["items-moveable"];
            if (Enum.TryParse(itemMoveableStr, true, out ItemsMoveable itemMoveable))
            {
                // Item must be in the room or inventory
                if (_advContext.IsItemAtCurrentLocation(itemMoveable))
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

using System;
using System.Linq;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using ColossalCave.Engine.Utilities;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.ActionHandlers
{
    public class MoveLocationHandler : BaseHandler, IMoveLocationHandler
    {
        public MoveLocationHandler(ILogger<MoveLocationHandler> log,
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
            _log.LogInformation("Handling move-location");

            var curLoc = _advContext.CurrentLocation;
            _log.LogInformation($"Current location: {curLoc}");
            var newLoc = curLoc;

            // movements
            var movementStr = _advContext.GetParameterValue("movements");
            // locations
            var locationStr = _advContext.GetParameterValue("locations");

            if (locationStr != null)
            {
                var exit = curLoc.Exits.FirstOrDefault(
                    x => x.Value.GoesTo.Name.EqualsNoCase(locationStr)).Value;
                if (exit != null)
                {
                    newLoc = exit.GoesTo;
                }
                else
                {
                    _responseBuilder.PrefixResponse(MsgMnemonic.MoveTooFarAway, 1);
                }
            }
            else
            {
                // Try to do the movement
                if (Enum.TryParse(movementStr, true, out Movements move))
                {
                }
                else
                    _responseBuilder.PrefixResponse(MsgMnemonic.VocabDontUnderstand, 1);
            }
            _log.LogInformation($"New location: {newLoc}");

            _mapHelper.Move(newLoc);
        }
    }
}

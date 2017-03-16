using System;
using System.Linq;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using ColossalCave.Engine.Utilities;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.Handlers
{
    public class MoveLocationHandler : BaseHandler, IMoveLocationHandler
    {
        public MoveLocationHandler(ILogger<MoveLocationHandler> log,
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
            _log.LogInformation("Handling move-location");

            var curLoc = _advHelper.CurrentLocation;
            _log.LogInformation($"Current location: {curLoc}");
            var newLoc = curLoc;

            // movements
            var movementStr = _advHelper.GetParameterValue("movements");
            // locations
            var locationStr = _advHelper.GetParameterValue("locations");

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

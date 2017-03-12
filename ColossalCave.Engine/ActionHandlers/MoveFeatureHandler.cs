using System;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.ActionHandlers
{
    public class MoveFeatureHandler : BaseHandler, IMoveFeatureHandler
    {
        public MoveFeatureHandler(ILogger<MoveFeatureHandler> log,
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
            _log.LogInformation("Handling move-feature");

            var curLoc = _advContext.CurrentLocation;
            _log.LogInformation($"Current location: {curLoc}");
            var newLoc = curLoc;

            // movements
            var movementStr = _advContext.GetParameterValue("movements");
            // locations
            var featureStr = _advContext.GetParameterValue("items-fixed");

            if (featureStr != null)
            {
                // TODO: Need implementation
            }
            else
            {
                // Try to do the movement
                if (Enum.TryParse(movementStr, true, out Movements move))
                {
                }
                else
                    _responseBuilder.PrefixResponse(Mnemonic.VocabDontUnderstand, 1);
            }
            _log.LogInformation($"New location: {newLoc}");

            _mapHelper.Move(curLoc, newLoc);
        }
    }
}

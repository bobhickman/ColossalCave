using System;
using System.Linq;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.ActionHandlers
{
    public class MoveDirectionHandler : BaseHandler, IMoveDirectionHandler
    {
        public MoveDirectionHandler(ILogger<MoveDirectionHandler> log,
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
            _log.LogInformation("Handling move-direction");

            var curLoc = _advContext.CurrentLocation;
            _log.LogInformation($"Current location: {curLoc}");
            var newLoc = curLoc;

            // movements
            var movementStr = _advContext.GetParameterValue("movements");
            // directions
            var directionStr = _advContext.GetParameterValue("directions");

            if (directionStr != null)
            {
                // Got a direction, just do it
                if (Enum.TryParse(directionStr, true, out Directions dir))
                {
                    if (curLoc.Exits.ContainsKey(dir))
                    {
                        var exit = curLoc.Exits[dir];
                        if (exit.GoesToRandom != null)
                            newLoc = RandomizeExit(exit);
                        else
                            newLoc = exit.GoesTo;
                    }
                    else
                        _responseBuilder.PrefixResponse(Mnemonic.CantGoThatWay, 1);
                }
                else
                    _responseBuilder.PrefixResponse(Mnemonic.DontUnderstand, 1);
            }
            else // No direction
            {
                // Try to do the movement
                if (Enum.TryParse(movementStr, true, out Movements move))
                    _responseBuilder.PrefixResponse(Mnemonic.NeedDirection, 1);
                else
                    _responseBuilder.PrefixResponse(Mnemonic.DontUnderstand, 1);
            }
            _log.LogInformation($"New location: {newLoc}");

            _mapHelper.Move(curLoc, newLoc);
        }

        private Location RandomizeExit(Exit exit)
        {
            var roll = new Random().Next(101);
            foreach (var re in exit.GoesToRandom)
                if (roll <= re.Probability)
                    return re.GoesTo;
            return exit.GoesToRandom.First().GoesTo;
        }

    }
}

using System;
using System.Linq;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.Handlers
{
    public class MoveDirectionHandler : BaseHandler, IMoveDirectionHandler
    {
        public MoveDirectionHandler(ILogger<MoveDirectionHandler> log,
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
            _log.LogInformation("Handling move-direction");

            var curLoc = _advHelper.CurrentLocation;
            _log.LogInformation($"Current location: {curLoc}");
            var newLoc = curLoc;

            // movements
            var movementStr = _advHelper.GetParameterValue("movements");
            // directions
            var directionStr = _advHelper.GetParameterValue("directions");

            if (directionStr != null)
            {
                // Got a direction, just do it
                if (Enum.TryParse(directionStr, true, out Directions dir))
                {
                    if (curLoc.Exits.ContainsKey(dir))
                    {
                        var exit = curLoc.Exits[dir];
                        if (!BlockCheck(curLoc, exit))
                        {
                            if (exit.GoesToRandom != null)
                                newLoc = RandomizeExit(exit);
                            else
                                newLoc = exit.GoesTo;
                        }
                    }
                    else
                        _responseBuilder.PrefixResponse(MsgMnemonic.MoveCantGoThatWay, 1);
                }
                else
                    _responseBuilder.PrefixResponse(MsgMnemonic.VocabDontUnderstand, 1);
            }
            else // No direction
            {
                // Try to do the movement
                if (Enum.TryParse(movementStr, true, out Movements move))
                    _responseBuilder.PrefixResponse(MsgMnemonic.MoveNeedDirection, 1);
                else
                    _responseBuilder.PrefixResponse(MsgMnemonic.VocabDontUnderstand, 1);
            }
            _log.LogInformation($"New location: {newLoc}");

            if (newLoc != curLoc)
                _mapHelper.Move(newLoc);
        }

        private Location RandomizeExit(Exit exit)
        {
            var roll = new Random().Next(101);
            foreach (var re in exit.GoesToRandom)
                if (roll <= re.Probability)
                    return re.GoesTo;
            return exit.GoesToRandom.First().GoesTo;
        }

        /// <summary>
        /// Check for and handle blocked exits.
        /// </summary>
        private bool BlockCheck(Location curLoc, Exit exit)
        {
            if (exit.Blocker != Items.Undefined)
            {
                if (exit.Blocker == Items.Grate)
                {
                    var state = _advHelper.GetItemState(Items.Grate, ItemState.GrateStatus);
                    if (state == 0) // closed, locked
                    {
                        _responseBuilder.AddToResponse(MsgMnemonic.GrateCantGoThrough);
                        return true;
                    }
                    else if (state == 1) // closed, unlocked
                    {
                        _responseBuilder.AddToResponse(MsgMnemonic.GrateIsClosed);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

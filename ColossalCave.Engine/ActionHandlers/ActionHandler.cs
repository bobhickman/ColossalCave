using System;
using System.Collections.Generic;
using System.Linq;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Interfaces;
using ColossalCave.Engine.Utilities;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.ActionHandlers
{
    public class ActionHandler : IActionHandler
    {
        private readonly ILogger _log;

        private readonly IResponseBuilder _responseBuilder;
        private readonly IMessageProvider _messageProvider;
        private readonly ILocationProvider _locationProvider;

        private AdventureContext _advContext;

        public ActionHandler(ILogger<ActionHandler> log,
            IResponseBuilder responseBuilder,
            IMessageProvider messageProvider,
            ILocationProvider locationProvider,
            AdventureContext context)
        {
            _log = log; 
            _responseBuilder = responseBuilder;
            _messageProvider = messageProvider;
            _locationProvider = locationProvider;
            _advContext = context;
        }

        public void Handle()
        {
            //var speech = $"I don't know how to {context.IntentName}";
            //if (context.IntentName == "addtoinventory")
            //    speech = Take(context.Parameters);
            if (_advContext.IntentName.EqualsNoCase("move-direction"))
                MoveDirection();
            else if (_advContext.IntentName.EqualsNoCase("move-location"))
                MoveLocation();
            //else if (_advContext.IntentName.EqualsNoCase("move-feature"))
            //    MoveFeature(_advContext);
            else if (_advContext.IntentName == "magic")
                Magic();
            //if (_advContext.IntentName == "lookaround")
            //    speech = Look(_advContext.Parameters);
            //_advContext.SpeechResponse = speech;

            _advContext.SpeechResponse = _responseBuilder.Speech;
            _advContext.TextResponse = _responseBuilder.Text;
        }

        private string Take(Dictionary<string, string> parameters)
        {
            _log.LogInformation("Handling Take");
            var action = parameters["actions"];
            var item = parameters
                .Where(p => p.Key != "actions")
                .First(p => !string.IsNullOrWhiteSpace(p.Value))
                .Value;
            return $"You {action} the {item}!";
        }

        private string Look(Dictionary<string, string> parameters)
        {
            _log.LogInformation("Handling Look");
            var look = parameters["visuals"];
            if (parameters.TryGetValue("directions", out string direction) && !string.IsNullOrWhiteSpace(direction))
                return $"You look {direction}. It's pitch black.";
            return $"You {look}. There's not much to see.";
        }

        private string GetParameterValue(string name)
        {
            var value = _advContext.Parameters.ContainsKey(name) 
                ? _advContext.Parameters[name] : null;
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value;
        }

        private void MoveDirection()
        {
            _log.LogInformation("Handling move-direction");

            var theMap = _locationProvider.Map;
            var curLoc = theMap[_advContext.CurrentLocationId];
            _log.LogInformation($"Current location: {curLoc}");
            var newLoc = curLoc;

            // movements
            var movementStr = GetParameterValue("movements");
            // directions
            var directionStr = GetParameterValue("directions");

            if (directionStr != null)
            {
                // Got a direction, just do it
                if (Enum.TryParse(directionStr, true, out Direction dir))
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
                        _responseBuilder.PrefixResponse(9, 1); // can't go that way
                }
                else
                    _responseBuilder.PrefixResponse(13, 1); // don't understand
            }
            else // No direction
            {
                // Try to do the movement
                if (Enum.TryParse(movementStr, true, out Movement move))
                    _responseBuilder.PrefixResponse(10, 1); // need direction
                else
                    _responseBuilder.PrefixResponse(13, 1); // don't understand
            }
            _log.LogInformation($"New location: {newLoc}");

            MakeMove(curLoc, newLoc);
        }

        private Location RandomizeExit(Exit exit)
        {
            var roll = new Random().Next(101);
            foreach (var re in exit.GoesToRandom)
                if (roll <= re.Probability)
                    return re.GoesTo;
            return exit.GoesToRandom.First().GoesTo;
        }

        private void MoveLocation()
        {
            _log.LogInformation("Handling move-location");

            var theMap = _locationProvider.Map;
            var curLoc = theMap[_advContext.CurrentLocationId];
            _log.LogInformation($"Current location: {curLoc}");
            var newLoc = curLoc;

            // movements
            var movementStr = GetParameterValue("movements");
            // locations
            var locationStr = GetParameterValue("locations");

            if (locationStr != null)
            {
                var exit = curLoc.Exits.FirstOrDefault(x => x.Value.GoesTo.Name.EqualsNoCase(locationStr)).Value;
                if (exit != null)
                {
                    newLoc = exit.GoesTo;
                }
                else
                {
                    _responseBuilder.PrefixResponse(1001, 1); // not close
                    //_messageProvider.GetMessage(12).Text, 1); // don't know how
                }
            }
            else
            {
                // Try to do the movement
                if (Enum.TryParse(movementStr, true, out Movement move))
                {
                }
                else
                    _responseBuilder.PrefixResponse(13, 1); // don't understand
            }
            _log.LogInformation($"New location: {newLoc}");

            MakeMove(curLoc, newLoc);
        }

        private void MakeMove(Location curLoc, Location newLoc)
        {
            if (newLoc.Id == 11) // TODO: Need dark check
                _advContext.Flags |= Flags.KnowsXYZZY;

            // Check if the new location has no exits. If so, add msg and go back to old loc.
            if (newLoc.Exits == null)
            {
                if (!newLoc.IsLight)
                {
                    _responseBuilder.AddToResponse(16, 1); // pitch dark
                    _responseBuilder.AddToResponse(
                        curLoc.Description,
                        curLoc.Description);
                }
                else
                {
                    _responseBuilder.AddToResponse(
                        newLoc.Description, 1,
                        newLoc.Description + "\n");
                    _responseBuilder.AddToResponse(
                        curLoc.Description,
                        curLoc.Description);
                }
                newLoc.Id = curLoc.Id;
                _advContext.CurrentLocationId = curLoc.Id;
            }
            else
            {
                if (newLoc.IsLight)
                {
                    _responseBuilder.AddToResponse(newLoc.Description);
                }
                else
                {
                    _responseBuilder.AddToResponse(16); // pitch dark
                }

                _advContext.CurrentLocationId = newLoc.Id;
            }
        }

        private void Magic()
        {
            _log.LogInformation("Handling Magic");

            var theMap = _locationProvider.Map;
            var curLoc = theMap[_advContext.CurrentLocationId];
            _log.LogInformation($"Current location: {curLoc}");
            var newLoc = curLoc;

            var magicStr = _advContext.Parameters["magicwords"];
            if (Enum.TryParse(magicStr, true, out Magicword magic))
            {
                if (magic == Magicword.xyzzy &&
                    _advContext.Flags.HasFlag(Flags.KnowsXYZZY) &&
                    (curLoc.Id == 3 || curLoc.Id == 11))
                {
                    newLoc = curLoc.Id == 3 ? theMap[11] : theMap[3];
                }
                else if (magic == Magicword.plugh &&
                    _advContext.Flags.HasFlag(Flags.KnowsPlugh) &&
                    (curLoc.Id == 3 || curLoc.Id == 33))
                {
                    newLoc = curLoc.Id == 3 ? theMap[33] : theMap[3];
                }
                else if (magic == Magicword.plover &&
                    _advContext.Flags.HasFlag(Flags.KnowsPlover) &&
                    (curLoc.Id == 100 || curLoc.Id == 33))
                {
                    newLoc = curLoc.Id == 100 ? theMap[33] : theMap[100];
                }
                //else if (magic == Magicword.fee)
                else
                {
                    _responseBuilder.PrefixResponse(42, 1); // Nothing happens
                }
            }
            else
                _responseBuilder.PrefixResponse(13, 1); // don't understand

            _log.LogInformation($"New location: {newLoc}");

            MakeMove(curLoc, newLoc);
        }
    }
}

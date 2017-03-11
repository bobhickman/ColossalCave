using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine
{
    public class MapHelper : IMapHelper
    {
        private readonly ILogger _log;

        private readonly IResponseBuilder _responseBuilder;
        private readonly ILocationProvider _locationProvider;

        private AdventureContext _advContext;

        public MapHelper(ILogger<MapHelper> log,
            IResponseBuilder responseBuilder,
            ILocationProvider locationProvider,
            AdventureContext context)
        {
            _log = log;
            _responseBuilder = responseBuilder;
            _locationProvider = locationProvider;
            _advContext = context;
        }

        public void Move(Location curLoc, Location newLoc)
        {
            if (newLoc.Id == 11 && newLoc.IsLight)
                _advContext.Flags |= Flags.KnowsXYZZY;

            // Check if the new location has no exits. 
            // If so, add msg and go back to old loc.
            if (newLoc.Exits == null)
            {
                if (!newLoc.IsLight)
                {
                    _responseBuilder.AddToResponse(Mnemonic.PitchDark, 1);
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
                _advContext.CurrentLocation = curLoc;
            }
            else
            {
                if (newLoc.IsLight)
                    _responseBuilder.AddToResponse(newLoc.Description);
                else
                    _responseBuilder.AddToResponse(Mnemonic.PitchDark);

                _advContext.CurrentLocation = newLoc;
            }
        }
    }
}

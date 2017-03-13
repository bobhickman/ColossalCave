using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine
{
    public class MapHelper : IMapHelper
    {
        private readonly ILogger _log;

        private readonly IResponseBuilder _responseBuilder;
        private readonly IItemProvider _itemProvider;
        private readonly ILocationProvider _locationProvider;

        private AdventureContext _advContext;

        public MapHelper(ILogger<MapHelper> log,
            IResponseBuilder responseBuilder,
            IItemProvider itemProvider,
            ILocationProvider locationProvider,
            AdventureContext context)
        {
            _log = log;
            _responseBuilder = responseBuilder;
            _itemProvider = itemProvider;
            _locationProvider = locationProvider;
            _advContext = context;
        }

        public bool Move(Location newLoc)
        {
            var curLoc = _advContext.CurrentLocation;
            var curLocLighted = _advContext.AdventurerHasALitLamp() || 
                _advContext.IsCurrentLocationLight();
            var newLocLighted = _advContext.AdventurerHasALitLamp() || 
                _advContext.IsLocationLight(newLoc);

            if (newLoc.Id == 11 && newLocLighted)
                _advContext.Flags |= AdventureContextFlags.KnowsXYZZY;

            bool isMoved = false;

            // Check if the new location has no exits. 
            // If so, add msg and go back to old loc.
            if (newLoc.Exits == null)
            {
                if (!newLocLighted)
                {
                    _responseBuilder.AddToResponse(MsgMnemonic.MovePitchDark, 1);
                }
                else
                {
                    _responseBuilder.AddToResponse(
                        newLoc.Description, 1,
                        newLoc.Description + "\n");
                }
                _responseBuilder.AddToResponse(
                    curLoc.Description,
                    curLoc.Description);
            }
            else
            {
                if (newLocLighted)
                {
                    _responseBuilder.AddToResponse(newLoc.Description);
                }
                else
                {
                    _responseBuilder.AddToResponse(MsgMnemonic.MovePitchDark);
                }
                isMoved = true;
                _advContext.CurrentLocation = newLoc;
                if (newLocLighted)
                    EnumerateItemsHere();
            }

            return isMoved;
        }

        public void EnumerateItemsHere()
        {
            var itemsHere = _advContext.GetItemsAtCurrentLocation();
            if (itemsHere.Count > 0)
                _responseBuilder.AddToResponse("", 1);
            foreach (var item in itemsHere)
            {
                var states = _advContext.GetItemStates(item.ItemEnum);
                if (states == null)
                {
                    _responseBuilder.AddToResponse(item.FoundDescriptions[0].Item2, 1);
                }
                else
                {
                    _responseBuilder.AddToResponse(_itemProvider.GetItemFoundDescription(item, states), 1);
                }
            }
        }
    }
}

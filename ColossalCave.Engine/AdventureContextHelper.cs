using System.Collections.Generic;
using System.Linq;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine
{
    public class AdventureContextHelper : IAdventureContextHelper
    {
        private readonly ILogger _log;

        private readonly IItemProvider _itemProvider;
        private readonly ILocationProvider _locationProvider;

        private AdventureContext _context;

        public AdventureContextHelper(ILogger<MapHelper> log,
            IItemProvider itemProvider,
            ILocationProvider locationProvider,
            AdventureContext context)
        {
            _log = log;
            _itemProvider = itemProvider;
            _locationProvider = locationProvider;
            _context = context;
        }

        #region Location management

        public Location CurrentLocation
        {
            get { return _context.CurrentLocation; }
            set { _context.CurrentLocation = value; }
        }

        public bool IsCurrentLocationLight => 
            IsLocationLight(_context.CurrentLocation);

        public bool IsLocationLight(Location location)
        {
            if (location.IsLight == true)
                return true;
            else if (IsItemAtLocation(ItemsMoveable.Lantern, location.Id) &&
                GetItemState(ItemsMoveable.Lantern, ItemState.LanternIsOn) == 1)
                return true;
            return false;
        }

        public bool AdventurerHasALitLamp =>
            IsItemInInventory(ItemsMoveable.Lantern) &&
                GetItemState(ItemsMoveable.Lantern, ItemState.LanternIsOn) == 1;

        #endregion

        #region Item state management

        public List<ItemStateValuePair> GetItemStates(ItemsMoveable item)
        {
            return _context.ItemsMoveableStates.ContainsKey(item) ? _context.ItemsMoveableStates[item] : null;
        }

        public int GetItemState(ItemsMoveable item, ItemState stateName)
        {
            return _context.ItemsMoveableStates[item]
                .Where(p => p.ItemStateName == stateName)
                .First()
                .Value;
        }

        public void SetItemState(ItemsMoveable item, ItemState stateName, int value)
        {
            var nvp = _context.ItemsMoveableStates[item]
                .Where(p => p.ItemStateName == stateName)
                .First();
            nvp.Value = value;
        }

        #endregion

        #region Item location management

        public bool IsItemInInventory(ItemsMoveable item)
        {
            return IsItemAtLocation(item, 0);
        }

        public bool IsItemAtCurrentLocation(ItemsMoveable item)
        {
            return IsItemAtLocation(item, _context.CurrentLocation.Id) || IsItemInInventory(item);
        }

        public List<Item> GetItemsAtCurrentLocation()
        {
            var result = new List<Item>();
            var itemMoveables = _context.ItemLocations
                .Where(il => il.Value == _context.CurrentLocation.Id)
                .Select(il => il.Key)
                .ToList();
            if (itemMoveables.Count > 0)
            {
                foreach (var im in itemMoveables)
                    result.Add(_itemProvider.GetItem(im));
            }
            return result;
        }

        public List<Item> GetInventory()
        {
            var result = new List<Item>();
            var itemMoveables = _context.ItemLocations
                .Where(il => il.Value == 0)
                .Select(il => il.Key)
                .ToList();
            if (itemMoveables.Count > 0)
            {
                foreach (var im in itemMoveables)
                    result.Add(_itemProvider.GetItem(im));
            }
            return result;
        }

        public bool IsItemAtLocation(ItemsMoveable item, int locationId)
        {
            if ((_context.ItemLocations[item] == locationId) ||
                (_context.CurrentLocation.Id == locationId && IsItemInInventory(item)))
                return true;
            return false;
        }

        public bool IsInventoryEmpty =>
            _context.ItemLocations.Where(il => il.Value == 0).Count() == 0; 

        public bool IsInventoryFull =>
            _context.ItemLocations.Where(il => il.Value == 0).Count() >= AdventureContext.MaxInventory; 

        public void AddToInventory(ItemsMoveable item)
        {
            _context.ItemLocations[item] = 0;
        }

        public void RemoveFromInventory(ItemsMoveable item)
        {
            MoveItemToLocation(item, _context.CurrentLocation.Id);
        }

        public void MoveItemToLocation(ItemsMoveable item, int locationId)
        {
            _context.ItemLocations[item] = locationId;
        }

        #endregion

        #region Parameter management

        public string GetParameterValue(string name)
        {
            var value = _context.Parameters.ContainsKey(name) ? _context.Parameters[name] : null;
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value;
        }

        #endregion

        #region Flags

        public bool HasFlag(AdventureContextFlags flag)
        {
            return _context.Flags.HasFlag(flag);
        }

        public void SetFlag(AdventureContextFlags flag)
        {
            _context.Flags |= flag;
        }

        #endregion
    }
}

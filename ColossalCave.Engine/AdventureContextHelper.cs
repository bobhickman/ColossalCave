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
            else if (IsItemAtLocation(Items.Lantern, location.Id) &&
                GetItemState(Items.Lantern, ItemState.LanternIsOn) == 1)
                return true;
            return false;
        }

        public bool AdventurerHasALitLamp =>
            IsItemInInventory(Items.Lantern) &&
                GetItemState(Items.Lantern, ItemState.LanternIsOn) == 1;

        #endregion

        #region Item state management

        public List<ItemStateValuePair> GetItemStates(Items item)
        {
            return _context.ItemStates.ContainsKey(item) ? _context.ItemStates[item] : null;
        }

        public int GetItemState(Items item, ItemState stateName)
        {
            if (_context.ItemStates.TryGetValue(item, out var itemStates))
            {
                var state = itemStates
                    .Where(p => p.ItemStateName == stateName)
                    .FirstOrDefault();
                if (state != null)
                    return state.Value;
            }
            return -1;
        }

        public void SetItemState(Items item, ItemState stateName, int value)
        {
            var nvp = _context.ItemStates[item]
                .Where(p => p.ItemStateName == stateName)
                .First();
            nvp.Value = value;
        }

        #endregion

        #region Item location management

        public bool IsItemInInventory(Items item)
        {
            return IsItemAtLocation(item, (int)LocMnemonics.Inventory);
        }

        public bool IsItemAtCurrentLocation(Items item)
        {
            return IsItemAtLocation(item, _context.CurrentLocation.Id) || IsItemInInventory(item);
        }

        public List<Item> GetItemsAtCurrentLocation()
        {
            var result = new List<Item>();
            var itemMoveables = _context.MoveableItemLocations
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
            var itemMoveables = _context.MoveableItemLocations
                .Where(il => il.Value == (int)LocMnemonics.Inventory)
                .Select(il => il.Key)
                .ToList();
            if (itemMoveables.Count > 0)
            {
                foreach (var im in itemMoveables)
                    result.Add(_itemProvider.GetItem(im));
            }
            return result;
        }

        public bool IsItemAtLocation(Items item, int locationId)
        {
            // Is it a moveable item which is at the current location?
            if (_context.MoveableItemLocations.TryGetValue(item, out var location))
            {
                if (location == locationId)
                    return true;
            }

            // Is the adventurer at the location and holding the item?
            if (_context.CurrentLocation.Id == locationId && IsItemInInventory(item))
                return true;

            // Is the item a fixed-location item and at the current location?
            var fixedItem = _itemProvider.GetItem(item);
            if (fixedItem.InitialLocationId == locationId ||
                fixedItem.LocationId2 == locationId)
                return true;

            return false;
        }

        public bool IsInventoryEmpty =>
            _context.MoveableItemLocations
                .Where(il => il.Value == (int)LocMnemonics.Inventory)
                .Count() == 0; 

        public bool IsInventoryFull =>
            _context.MoveableItemLocations
                .Where(il => il.Value == (int)LocMnemonics.Inventory)
                .Count() >= AdventureContext.MaxInventory; 

        public void AddToInventory(Items item)
        {
            _context.MoveableItemLocations[item] = (int)LocMnemonics.Inventory;
        }

        public void RemoveFromInventory(Items item)
        {
            MoveItemToLocation(item, _context.CurrentLocation.Id);
        }

        public void MoveItemToLocation(Items item, int locationId)
        {
            _context.MoveableItemLocations[item] = locationId;
        }

        #endregion

        #region Item descriptions

        public string GetItemExamination(Items itemEnum)
        {
            var item = _itemProvider.GetItem(itemEnum);
            return _itemProvider.GetItemExamineDescription(item, GetItemStates(itemEnum));
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

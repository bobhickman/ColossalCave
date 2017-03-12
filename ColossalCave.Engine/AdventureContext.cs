using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine
{
    public class AdventureContext
    {
        public const int MaxInventory = 7;

        //private readonly ILogger _log;
        private readonly IItemProvider _itemProvider;
        private readonly ILocationProvider _locationProvider;

        public string ContextId { get; set; }

        /// <summary>
        /// Up to 31 flags for adventure state
        /// </summary>
        public AdventureContextFlags Flags { get; set; }

        public AdventureContext(//ILogger log,
            IItemProvider itemProvider,
            ILocationProvider locationProvider)
        {
            //_log = log;
            _itemProvider = itemProvider;
            _locationProvider = locationProvider;

            // Initialize everything as if game is new
            SetCurrentLocation(1);

            _itemLocations = new Dictionary<ItemsMoveable, int>();
            foreach(var itemLoc in _itemProvider.Items)
                _itemLocations.Add(itemLoc.ItemEnum, itemLoc.InitialLocationId);
        }

        #region Inputs

        public string IntentName { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        #endregion

        #region Outputs

        public string SpeechResponse { get; set; }

        public string TextResponse { get; set; }

        #endregion

        #region Location management

        /// <summary>
        /// The current location of the adventurer
        /// </summary>
        public Location CurrentLocation { get; set; }

        public void SetCurrentLocation(int locationId)
        {
            CurrentLocation = _locationProvider.GetLocation(locationId);
        }

        #endregion
        
        #region Item management

        private Dictionary<ItemsMoveable,int> _itemLocations { get; set; }

        public bool IsItemInInventory(ItemsMoveable item)
        {
            return IsItemAtLocation(item, 0);
        }

        public bool IsItemAtCurrentLocation(ItemsMoveable item)
        {
            return IsItemAtLocation(item, CurrentLocation.Id);
        }

        public List<Item> GetItemsAtCurrentLocation()
        {
            var result = new List<Item>();
            var itemMoveables = _itemLocations
                .Where(il => il.Value == CurrentLocation.Id)
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
            return _itemLocations[item] == locationId;
        }

        public bool IsInventoryEmpty
        {
            get { return _itemLocations.Where(il => il.Value == 0).Count() == 0; }
        }

        public bool IsInventoryFull
        {
            get { return _itemLocations.Where(il => il.Value == 0).Count() >= MaxInventory; }
        }

        public void AddToInventory(ItemsMoveable item)
        {
            _itemLocations[item] = 0;
        }

        public void RemoveItemFromInventory(ItemsMoveable item)
        {
            MoveItemToLocation(item, CurrentLocation.Id);
        }

        public void MoveItemToLocation(ItemsMoveable item, int locationId)
        {
            _itemLocations[item] = locationId;
        }

        public string ItemLocationsToString()
        {
            // "itemid,locid|itemid,locid|itemid,locid"
            var buf = new StringBuilder();
            foreach(var il in _itemLocations)
                buf.Append($"{il.Key},{il.Value}|");
            return buf.ToString(0, buf.Length - 1);
        }

        public void ItemLocationsFromString(string str)
        {
            // "itemid,locid|itemid,locid|itemid,locid"
            _itemLocations = new Dictionary<ItemsMoveable, int>();
            var itemStrs = str.Split('|');
            foreach (var istr in itemStrs)
            {
                var pieces = istr.Split(',');
                if (Enum.TryParse(pieces[0], true, out ItemsMoveable item))
                    _itemLocations.Add(item, int.Parse(pieces[1]));
            }
        }

        #endregion

        #region Parameter management

        public string GetParameterValue(string name)
        {
            var value = Parameters.ContainsKey(name) ? Parameters[name] : null;
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value;
        }

        #endregion
    }

    [Flags]
    public enum AdventureContextFlags
    {
        None = 0x00000000,
        KnowsXYZZY = 0x00000001,
        KnowsPlugh = 0x00000002,
        KnowsPlover = 0x00000004,
        KnowsFee = 0x00000008,
        GrateIsUnlocked = 0x00000010,
        GrateIsOpen = 0x00000020,
        LanternIsOn = 0x00000040,
        RodIsMarked = 0x00000080,
        Bit09 = 0x00000100,
        Bit10 = 0x00000200,
        Bit11 = 0x00000400,
        Bit12 = 0x00000800,
        Bit13 = 0x00001000,
        Bit14 = 0x00002000,
        Bit15 = 0x00004000,
        Bit16 = 0x00008000,
        Bit17 = 0x00010000,
        Bit18 = 0x00020000,
        Bit19 = 0x00040000,
        Bit20 = 0x00080000,
        Bit21 = 0x00100000,
        Bit22 = 0x00200000,
        Bit23 = 0x00400000,
        Bit24 = 0x00800000,
        Bit25 = 0x01000000,
        Bit26 = 0x02000000,
        Bit27 = 0x04000000,
        Bit28 = 0x08000000,
        Bit29 = 0x10000000,
        Bit30 = 0x20000000,
        Bit31 = 0x40000000
        //Bit32 = 0x80000000 // Not usable
    }
}

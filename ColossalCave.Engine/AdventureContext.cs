using System;
using System.Collections.Generic;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Newtonsoft.Json;

namespace ColossalCave.Engine
{
    public class AdventureContext
    {
        public const int MaxInventory = 7;

        private readonly IItemProvider _itemProvider;
        private readonly ILocationProvider _locationProvider;

        public string ContextId { get; set; }

        /// <summary>
        /// Up to 31 flags for adventure state
        /// </summary>
        public AdventureContextFlags Flags { get; set; }

        public AdventureContext(
            IItemProvider itemProvider,
            ILocationProvider locationProvider)
        {
            _itemProvider = itemProvider;
            _locationProvider = locationProvider;

            // Initialize everything as if game is new
            SetCurrentLocation(1);

            MoveableItemLocations = new Dictionary<Items, int>();
            foreach(var itemLoc in _itemProvider.Items)
                MoveableItemLocations.Add(itemLoc.ItemEnum, itemLoc.InitialLocationId);

            ItemStates = new Dictionary<Items, List<ItemStateValuePair>>();
            foreach(var im in _itemProvider.Items)
            {
                if (im.DefaultStates != null)
                {
                    ItemStates[im.ItemEnum] = new List<ItemStateValuePair>();
                    foreach (var pair in im.DefaultStates)
                        ItemStates[im.ItemEnum].Add(pair);
                }
            }
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

        #region Item state management

        // States of all items moveable and fixed, mobs and treasures
        public Dictionary<Items, List<ItemStateValuePair>> ItemStates;
        //private Dictionary<ItemsFixed, List<NameValuePair>> _itemsFixedStates;
        //private Dictionary<Mobs, List<NameValuePair>> _mobsStates;
        //private Dictionary<Treasures, List<NameValuePair>> _treasuresStates;

        public string StatesToJson()
        {
            return JsonConvert.SerializeObject(ItemStates);
        }

        public void StatesFromJson(string json)
        {
            ItemStates = JsonConvert.DeserializeObject<Dictionary<Items, List<ItemStateValuePair>>>(json);
        }

        #endregion

        #region Item location management

        // Locations of moveable items 
        public Dictionary<Items, int> MoveableItemLocations;

        public string ItemLocationsToJson()
        {
            return JsonConvert.SerializeObject(MoveableItemLocations);
        }

        public void ItemLocationsFromJson(string json)
        {
            MoveableItemLocations = JsonConvert.DeserializeObject<Dictionary<Items, int>>(json);
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
        Bit05 = 0x00000010,
        Bit06 = 0x00000020,
        Bit07 = 0x00000040,
        Bit08 = 0x00000080,
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

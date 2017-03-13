using System;
using System.Collections.Generic;
using System.Text;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Enumerations;

namespace ColossalCave.Engine.Interfaces
{
    public interface IAdventureContextHelper
    {
        Location CurrentLocation { get; set; }

        bool IsCurrentLocationLight { get; }

        bool IsLocationLight(Location location);

        bool AdventurerHasALitLamp { get; }

        List<ItemStateValuePair> GetItemStates(ItemsMoveable item);

        int GetItemState(ItemsMoveable item, ItemState stateName);

        void SetItemState(ItemsMoveable item, ItemState stateName, int value);

        bool IsItemInInventory(ItemsMoveable item);

        bool IsItemAtCurrentLocation(ItemsMoveable item);

        List<Item> GetItemsAtCurrentLocation();

        List<Item> GetInventory();

        bool IsItemAtLocation(ItemsMoveable item, int locationId);

        bool IsInventoryEmpty { get; }

        bool IsInventoryFull { get; }

        void AddToInventory(ItemsMoveable item);

        void RemoveFromInventory(ItemsMoveable item);

        void MoveItemToLocation(ItemsMoveable item, int locationId);

        string GetParameterValue(string name);

        bool HasFlag(AdventureContextFlags flag);

        void SetFlag(AdventureContextFlags flag);
    }
}

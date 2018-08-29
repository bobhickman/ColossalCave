using System;
using System.Collections.Generic;
using System.Text;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Enumerations;

namespace ColossalCave.Engine.Interfaces
{
    public interface IAdventureContextHelper
    {
        #region Location management

        Location CurrentLocation { get; set; }

        bool IsCurrentLocationLight { get; }

        bool IsLocationLight(Location location);

        bool AdventurerHasALitLamp { get; }

        #endregion

        #region Item state management

        List<ItemStateValuePair> GetItemStates(Items item);

        int GetItemState(Items item, ItemState stateName);

        void SetItemState(Items item, ItemState stateName, int value);

        #endregion

        #region Item location management

        bool IsItemInInventory(Items item);

        bool IsItemAtCurrentLocation(Items item);

        List<Item> GetItemsAtCurrentLocation();

        List<Item> GetInventory();

        bool IsItemAtLocation(Items item, int locationId);

        bool IsInventoryEmpty { get; }

        bool IsInventoryFull { get; }

        void AddToInventory(Items item);

        void RemoveFromInventory(Items item);

        void MoveItemToLocation(Items item, int locationId);

        #endregion

        #region Item descriptions

        string GetItemExamination(Items itemEnum);

        #endregion

        #region Parameter management 

        string GetParameterValue(string name);
        
        #endregion

        #region Flags

        bool HasFlag(AdventureContextFlags flag);

        void SetFlag(AdventureContextFlags flag);

        #endregion
    }
}

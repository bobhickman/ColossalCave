using System.Collections.Generic;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Enumerations;

namespace ColossalCave.Engine.Interfaces
{
    public interface IItemProvider
    {
        List<Item> Items { get; }

        Item GetItem(int id);

        Item GetItem(ItemsMoveable item);

        string GetItemFoundDescription(Item item, List<ItemStateValuePair> states);

        string GetItemExamineDescription(Item item, List<ItemStateValuePair> states);
    }
}

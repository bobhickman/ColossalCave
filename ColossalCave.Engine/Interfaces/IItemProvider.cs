using System.Collections.Generic;
using ColossalCave.Engine.AssetModels;

namespace ColossalCave.Engine.Interfaces
{
    public interface IItemProvider
    {
        List<Item> Items { get; }

        Item GetItem(int id);

        Item GetItem(ItemsMoveable item);
    }
}

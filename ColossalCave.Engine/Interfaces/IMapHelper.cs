using ColossalCave.Engine.AssetModels;

namespace ColossalCave.Engine.Interfaces
{
    public interface IMapHelper
    {
        bool Move(Location newLoc);

        void EnumerateItemsHere();
    }
}

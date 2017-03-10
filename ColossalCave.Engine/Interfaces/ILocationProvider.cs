using ColossalCave.Engine.AssetModels;
using System.Collections.Generic;

namespace ColossalCave.Engine.Interfaces
{
    public interface ILocationProvider
    {
        Dictionary<int,Location> Map { get; }

        Location GetLocation(int id);
    }
}

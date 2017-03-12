
using System;
using System.Collections.Generic;

namespace ColossalCave.Engine.AssetModels
{
    public class Item : Asset
    {
        // Notes on text:
        // Name should match ItemsMoveable enum and api.ai entity
        // ShortName is used in the inventory list so should stand alone e.g. Brass lantern
        // Description is used in examination and found list e.g. shiny brass lamp

        public ItemsMoveable ItemEnum { get; set; }

        public int InitialLocationId { get; set; }

        public List<Tuple<AdventureContextFlags, bool, string>> FoundDescriptions { get; set; }
    }
}

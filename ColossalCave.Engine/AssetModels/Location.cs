using System.Collections.Generic;
using ColossalCave.Engine.Enumerations;
using Newtonsoft.Json;

namespace ColossalCave.Engine.AssetModels
{
    public class Location : Asset
    {
        // NOTE: Location name should map to the fast-travel name from advent.dat
        // But also keep in mind everything can be synonymed in api.ai

        /// <summary>
        /// Mnemonic for this location
        /// </summary>
        [JsonProperty(DefaultValueHandling =DefaultValueHandling.Ignore)]
        public LocMnemonics Mnemonic { get; set; }

        /// <summary>
        /// Dict of directions to exits
        /// These are 'normal' exits from the location invoked e.g. 'Go west'
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<Directions, Exit> Exits { get; set; }

        /// <summary>
        /// Locations that can be instantly traveled to.
        /// These nearby locations can be traveled to by e.g. 'Go to the building'
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Location> FastTravel { get; set; }

        /// <summary>
        /// True if the location has natural lighting
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsLight { get; set; }

        /// <summary>
        /// Allow a location to be created only by specifying its id.
        /// Useful for building the entire object travel map.
        /// </summary>
        /// <param name="id"></param>
        public Location(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Location() { }
    }
}

using System.Collections.Generic;
using ColossalCave.Engine.Enumerations;
using Newtonsoft.Json;

namespace ColossalCave.Engine.AssetModels
{
    public class Location : Asset
    {
        // NOTE: Location name should map to the fast-travel name from advent.dat
        // But also keep in mind everything can be synonymed in api.ai

        // Dict of directions to exits
        // These are 'normal' exits from the location invoked e.g. 'Go west'
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<Directions, Exit> Exits { get; set; }

        // Locations that can be instantly traveled to.
        // These nearby locations can be traveled to by e.g. 'Go to the building'
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Location> FastTravel { get; set; }

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

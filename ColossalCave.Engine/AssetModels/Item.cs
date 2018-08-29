using System;
using System.Collections.Generic;
using ColossalCave.Engine.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ColossalCave.Engine.AssetModels
{
    public class Item : Asset
    {
        // Notes on text:
        // Name should match ItemsMoveable enum and api.ai entity
        // ShortDescription is used in the inventory list so should stand alone e.g. A shiny brass lantern
        // Description is used in examination e.g. The lamp is brass and very shiny.

        /// <summary>
        /// The item enum
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Items ItemEnum { get; set; }

        /// <summary>
        /// Location of the item at game start
        /// </summary>
        public int InitialLocationId { get; set; }

        /// <summary>
        /// An alternate location for the item.
        /// Useful for door items that exist in two locations.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int LocationId2 { get; set; }

        /// <summary>
        /// Starting states properties of the item.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ItemStateValuePair> DefaultStates { get; set; }

        /// <summary>
        /// Stateful descriptions of the item when found in a room
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Tuple<ItemStateValuePair, string>> FoundDescriptions { get; set; }

        /// <summary>
        /// Stateful descriptions of the item when examined.
        /// If the item has no states the base Description is used and this property will be null.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Tuple<ItemStateValuePair, string>> ExamineDescriptions { get; set; }
    }
}

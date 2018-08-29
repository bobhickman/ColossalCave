using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColossalCave.Engine.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ColossalCave.Engine.AssetModels
{
    public class Exit : Asset
    {
        /// <summary>
        /// Reference to the location that the exit leads to
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Location GoesTo { get; set; }

        /// <summary>
        /// An item which is initially blocking the exit
        /// </summary>
        [JsonProperty(DefaultValueHandling =DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Items Blocker { get; set; }

        /// <summary>
        /// Sets up a random exit.
        /// Item1 is the probability, Item2 is the location id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<RandomExit> GoesToRandom { get; set; }

        public Exit(int id, Items blocker = Items.Undefined)
        {
            Id = id;
            Blocker = blocker;
        }

        public Exit() { }
    }

    public class RandomExit
    {
        public int Probability { get; set; }
        public Location GoesTo { get; set; }

        public override string ToString()
        {
            return $"{Probability}:{GoesTo}";
        }
    }
}

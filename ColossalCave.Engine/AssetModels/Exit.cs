using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        /// A list of items that are needed to go through the exit
        /// </summary>
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public List<int> RequiredItems;

        /// <summary>
        /// Sets up a random exit.
        /// Item1 is the probability, Item2 is the location id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<RandomExit> GoesToRandom { get; set; }

        public Exit(int id)
        {
            Id = id;
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

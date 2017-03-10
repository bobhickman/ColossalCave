using Newtonsoft.Json;

namespace ColossalCave.Engine.AssetModels
{
    public class Asset
    {
        /// <summary>
        /// Unique id of the asset
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A unique single word name for the asset e.g. "road"
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Brief description of the asset e.g. "End of road"
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Verbose description of the asset
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Id}:{Name}";
        }
    }
}

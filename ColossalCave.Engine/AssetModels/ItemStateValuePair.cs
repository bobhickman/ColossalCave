using ColossalCave.Engine.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ColossalCave.Engine.AssetModels
{
    public class ItemStateValuePair
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ItemState ItemStateName { get; set; }

        public int Value { get; set; }

        public ItemStateValuePair() { }

        public ItemStateValuePair(ItemState itemStateName, int value)
        {
            ItemStateName = itemStateName;
            Value = value;
        }

        public override string ToString()
        {
            return $"{ItemStateName}:{Value}";
        }
    }
}

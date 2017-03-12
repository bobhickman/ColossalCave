using System;
using System.Collections.Generic;
using System.Text;
using ColossalCave.Engine.Enumerations;

namespace ColossalCave.Engine.AssetModels
{
    public class ItemStateValuePair
    {
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

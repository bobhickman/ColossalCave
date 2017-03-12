namespace ColossalCave.Engine.AssetModels
{
    public class NameValuePair
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public NameValuePair() { }

        public NameValuePair(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name}:{Value}";
        }
    }
}

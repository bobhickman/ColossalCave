using Newtonsoft.Json;

namespace ColossalCave.Engine.AssetModels
{
    public class Message
    {
        public int Id { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Speech { get; set; }

        public string Text { get; set; }

        [JsonIgnore]
        public string SpeechOrText { get { return Speech ?? Text; } }

        [JsonIgnore]
        public string TextOrSpeech { get { return Text ?? Speech; } }

        public override string ToString()
        {
            return Text;
        }
    }
}

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Interfaces;
using Newtonsoft.Json;

namespace ColossalCave.Engine.AssetProviders
{
    public class MessageProvider : IMessageProvider
    {
        private Dictionary<int, Message> _messages;

        public Message GetMessage(int id)
        {
            if (_messages == null)
                LoadMessages();
            return _messages[id];
        }

        private void LoadMessages()
        {
            Trace.TraceInformation("Loading message assets...");
            var assembly = typeof(MessageProvider).GetTypeInfo().Assembly;
            string[] names = assembly.GetManifestResourceNames();
            var stream = assembly.GetManifestResourceStream("ColossalCave.Engine.Assets.messages.json");
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var json = reader.ReadToEnd();
                _messages = JsonConvert.DeserializeObject<Dictionary<int,Message>>(json);
            }
        }
    }
}

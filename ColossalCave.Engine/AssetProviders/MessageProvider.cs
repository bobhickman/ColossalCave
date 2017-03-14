using System.Collections.Generic;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ColossalCave.Engine.AssetProviders
{
    public class MessageProvider : IMessageProvider
    {
        private readonly ILogger _log;
        private readonly IResourceLoader _resourceLoader;

        private Dictionary<int, Message> _messages;

        public MessageProvider(ILogger<MessageProvider> log,
            IResourceLoader resourceLoader)
        {
            _log = log;
            _resourceLoader = resourceLoader;
        }

        public Message GetMessage(int id)
        {
            if (_messages == null)
                LoadMessages();
            return _messages[id];
        }

        private void LoadMessages()
        {
            _log?.LogInformation("Loading message assets...");
            var json = _resourceLoader.LoadAsset("messages.json");
            _messages = JsonConvert.DeserializeObject<Dictionary<int, Message>>(json);
        }
    }
}

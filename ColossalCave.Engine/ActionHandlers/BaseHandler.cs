using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.ActionHandlers
{
    public abstract class BaseHandler : IActionHandler
    {
        protected readonly ILogger _log;

        protected readonly IResponseBuilder _responseBuilder;
        protected readonly IMessageProvider _messageProvider;
        protected readonly ILocationProvider _locationProvider;

        protected readonly IAdventureContextHelper _advHelper;
        protected readonly IMapHelper _mapHelper;

        public BaseHandler(ILogger<BaseHandler> log,
            IResponseBuilder responseBuilder,
            IMessageProvider messageProvider,
            ILocationProvider locationProvider,
            IAdventureContextHelper advHelper,
            IMapHelper mapHelper)
        {
            _log = log;
            _responseBuilder = responseBuilder;
            _messageProvider = messageProvider;
            _locationProvider = locationProvider;
            _advHelper = advHelper;
            _mapHelper = mapHelper;
        }

        public abstract void Handle();
    }
}

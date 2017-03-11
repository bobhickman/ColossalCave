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
        protected readonly IMapHelper _mapHelper;

        protected AdventureContext _advContext;

        public BaseHandler(ILogger<BaseHandler> log,
            IResponseBuilder responseBuilder,
            IMessageProvider messageProvider,
            ILocationProvider locationProvider,
            IMapHelper mapHelper,
            AdventureContext context)
        {
            _log = log;
            _responseBuilder = responseBuilder;
            _messageProvider = messageProvider;
            _locationProvider = locationProvider;
            _mapHelper = mapHelper;
            _advContext = context;
        }

        public abstract void Handle();
    }
}

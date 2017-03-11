using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.ActionHandlers
{
    public class InventoryHandler : BaseHandler, IInventoryHandler
    {
        public InventoryHandler(ILogger<InventoryHandler> log,
            IResponseBuilder responseBuilder,
            IMessageProvider messageProvider,
            ILocationProvider locationProvider,
            IMapHelper mapHelper,
            AdventureContext context)
            : base(log, responseBuilder, messageProvider, locationProvider, mapHelper, context)
        {
        }

        public override void Handle()
        {
            _log.LogInformation("Handling AddToInventory");
            var actionStr = _advContext.GetParameterValue("actions");
            var itemStr = _advContext.GetParameterValue("items");
            _responseBuilder.AddToResponse($"I don't know how to {actionStr} the {itemStr} yet. But I will soon!");
        }
    }
}

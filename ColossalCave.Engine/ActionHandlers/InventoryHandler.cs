using System;
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
            if (Enum.TryParse<Actions>(actionStr, true, out var action))
            {
                _log?.LogInformation($"Intent is {action}");
                var itemStr = _advContext.GetParameterValue("items-moveable");
                if (Enum.TryParse(itemStr, true, out ItemsMoveable item))
                {
                    _log?.LogInformation($"Target item is {item}");
                    if (action == Actions.Take)
                    {
                        if (_advContext.IsItemInInventory(item))
                        {
                            _responseBuilder.AddToResponse(Mnemonic.InvAlreadyCarrying);
                        }
                        else
                        {
                            if (!_advContext.IsItemAtCurrentLocation(item))
                            {
                                _responseBuilder.AddToResponse(Mnemonic.ItemNotHere);
                            }
                            else if (_advContext.IsInventoryFull)
                            {
                                _responseBuilder.AddToResponse(Mnemonic.InvFull);
                            }
                            else
                            {
                                _advContext.AddToInventory(item);
                                _responseBuilder.AddToResponse(Mnemonic.OK);
                            }
                        }
                    }
                    else if (action == Actions.Drop)
                    {
                        if (_advContext.IsItemInInventory(item))
                        {
                            _advContext.RemoveItemFromInventory(item);
                            _responseBuilder.AddToResponse(Mnemonic.OK);
                        }
                        else if (_advContext.IsInventoryEmpty)
                        {
                            _responseBuilder.AddToResponse(Mnemonic.InvNotCarryingAnything);
                        }
                        else
                        {
                            _responseBuilder.AddToResponse(Mnemonic.InvNotCarrying);
                        }
                    }
                }
                else
                {
                    _log.LogError($"Unknown inventory item: {itemStr}");
                    _responseBuilder.AddToResponse(Mnemonic.VocabDontUnderstand);
                }
            }
            else
            {
                _log.LogError($"Unknown action: {actionStr}");
                _responseBuilder.AddToResponse(Mnemonic.VocabDontUnderstand);
            }
        }
    }
}

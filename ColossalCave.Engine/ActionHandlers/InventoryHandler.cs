using System;
using ColossalCave.Engine.Enumerations;
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
                            _responseBuilder.AddToResponse(MsgMnemonic.InvAlreadyCarrying);
                        }
                        else
                        {
                            if (!_advContext.IsItemAtCurrentLocation(item))
                            {
                                _responseBuilder.AddToResponse(MsgMnemonic.ItemNotHere);
                            }
                            else if (_advContext.IsInventoryFull)
                            {
                                _responseBuilder.AddToResponse(MsgMnemonic.InvFull);
                            }
                            else
                            {
                                _advContext.AddToInventory(item);
                                _responseBuilder.AddToResponse(MsgMnemonic.OK);
                            }
                        }
                    }
                    else if (action == Actions.Drop)
                    {
                        if (_advContext.IsItemInInventory(item))
                        {
                            _advContext.RemoveFromInventory(item);
                            _responseBuilder.AddToResponse(MsgMnemonic.OK);
                        }
                        else if (_advContext.IsInventoryEmpty)
                        {
                            _responseBuilder.AddToResponse(MsgMnemonic.InvNotCarryingAnything);
                        }
                        else
                        {
                            _responseBuilder.AddToResponse(MsgMnemonic.InvNotCarrying);
                        }
                    }
                }
                else
                {
                    _log.LogError($"Unknown inventory item: {itemStr}");
                    _responseBuilder.AddToResponse(MsgMnemonic.VocabDontUnderstand);
                }
            }
            else
            {
                _log.LogError($"Unknown action: {actionStr}");
                _responseBuilder.AddToResponse(MsgMnemonic.VocabDontUnderstand);
            }
        }
    }
}

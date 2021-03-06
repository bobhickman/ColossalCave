﻿using System;
using System.Linq;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.Handlers
{
    public class InventoryHandler : BaseHandler, IInventoryHandler
    {
        public InventoryHandler(ILogger<InventoryHandler> log,
           IResponseBuilder responseBuilder,
           IMessageProvider messageProvider,
           ILocationProvider locationProvider,
           IAdventureContextHelper advHelper,
           IMapHelper mapHelper,
           AdventureContext context)
            : base(log, responseBuilder, messageProvider, locationProvider, advHelper, mapHelper)
        {
        }

        public override void Handle()
        {
            _log.LogInformation("Handling AddToInventory");

            var actionStr = _advHelper.GetParameterValue("actions-inventory");
            if (Enum.TryParse<Actions>(actionStr, true, out var action))
            {
                _log?.LogInformation($"Intent is {action}");
                var itemStr = _advHelper.GetParameterValue("items-moveable");
                if (itemStr == null)
                    itemStr = _advHelper.GetParameterValue("treasures");
                if (itemStr == null)
                    itemStr = _advHelper.GetParameterValue("items-fixed");
                if (itemStr == null)
                    itemStr = _advHelper.GetParameterValue("mobs");
                if (Enum.TryParse(itemStr, true, out Items targetItem))
                {
                    _log?.LogInformation($"Target item is {targetItem}");
                    if (action == Actions.Take)
                    {
                        if (targetItem == Items.All)
                        {
                            var itemsHere = _advHelper.GetItemsAtCurrentLocation();
                            if (itemsHere.Any())
                            {
                                foreach(var itemToPickup in itemsHere)
                                {
                                    if (_advHelper.IsInventoryFull)
                                    {
                                        _responseBuilder.AddToResponse(MsgMnemonic.InvFull);
                                        break;
                                    }
                                    else if (_advHelper.GetItemState(itemToPickup.ItemEnum, ItemState.IsFixed) == 1)
                                    {
                                        // ignore
                                        //_responseBuilder.AddToResponse($"You can't pick up the {itemToPickup.Name}");
                                    }
                                    else
                                    {
                                        _advHelper.AddToInventory(itemToPickup.ItemEnum);
                                        _responseBuilder.AddToResponse($"You pick up {itemToPickup.ShortDescription.ToLower()}. ", 1);
                                    }
                                }
                            }
                            else
                            {
                                _responseBuilder.AddToResponse("There's nothing here to take.");
                            }
                        }
                        else if (_advHelper.GetItemState(targetItem, ItemState.IsFixed) == 1)
                        {
                            _responseBuilder.AddToResponse($"You can't pick up the {targetItem}.");
                        }
                        else if (_advHelper.IsItemInInventory(targetItem))
                        {
                            _responseBuilder.AddToResponse(MsgMnemonic.InvAlreadyCarrying);
                        }
                        else // try pick it up
                        {
                            if (!_advHelper.IsItemAtCurrentLocation(targetItem))
                            {
                                _responseBuilder.AddToResponse(MsgMnemonic.ItemNotHere);
                            }
                            else if (_advHelper.IsInventoryFull)
                            {
                                _responseBuilder.AddToResponse(MsgMnemonic.InvFull);
                            }
                            else
                            {
                                _advHelper.AddToInventory(targetItem);
                                _responseBuilder.AddToResponse(MsgMnemonic.OK);
                            }
                        }
                    }
                    else if (action == Actions.Drop)
                    {
                        if (targetItem == Items.All)
                        {
                            if (!_advHelper.IsInventoryEmpty)
                            {
                                foreach(var itemToDrop in _advHelper.GetInventory())
                                {
                                    _advHelper.RemoveFromInventory(itemToDrop.ItemEnum);
                                    _responseBuilder.AddToResponse($"You drop {itemToDrop.ShortDescription.ToLower()}. ", 1);
                                }
                            }
                            else
                            {
                                _responseBuilder.AddToResponse(MsgMnemonic.InvNotCarryingAnything);
                            }
                        }
                        else if (_advHelper.IsItemInInventory(targetItem))
                        {
                            _advHelper.RemoveFromInventory(targetItem);
                            _responseBuilder.AddToResponse(MsgMnemonic.OK);
                        }
                        else if (_advHelper.IsInventoryEmpty)
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
                    _log.LogError($"Unknown item: {itemStr}");
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

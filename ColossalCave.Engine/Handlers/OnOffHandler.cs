using System;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.Handlers
{
    public class OnOffHandler : BaseHandler, IOnOffHandler
    {
        public OnOffHandler(ILogger<OnOffHandler> log,
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
            _log.LogInformation("Handling on-off");

            var actionStr = _advHelper.GetParameterValue("actions-on-off");
            if (Enum.TryParse<Actions>(actionStr, true, out var action))
            {
                var itemStr = _advHelper.GetParameterValue("items-moveable");
                if (itemStr == null)
                    itemStr = _advHelper.GetParameterValue("treasures");
                if (itemStr == null)
                    itemStr = _advHelper.GetParameterValue("items-fixed");
                if (Enum.TryParse(itemStr, true, out Items targetItem))
                {
                    _log?.LogInformation($"Target item is {targetItem}");
                    if (action == Actions.On)
                    {
                        if (targetItem == Items.Lantern)
                        {
                            if (_advHelper.IsItemInInventory(Items.Lantern))
                            {
                                if (_advHelper.GetItemState(Items.Lantern, ItemState.LanternIsOn) == 1)
                                    _responseBuilder.AddToResponse("The lamp is already on.");
                                else
                                {
                                    _advHelper.SetItemState(Items.Lantern, ItemState.LanternIsOn, 1);
                                    _responseBuilder.AddToResponse(MsgMnemonic.LampIsNowOn);
                                }
                            }
                            else
                            {
                                _responseBuilder.AddToResponse(MsgMnemonic.InvNotCarrying);
                            }
                        }
                        else
                        {
                            _responseBuilder.AddToResponse($"You can't {actionStr} the {itemStr}.");
                        }
                    }
                    else if (action == Actions.Off)
                    {
                        if (targetItem == Items.Lantern)
                        {
                            if (_advHelper.IsItemInInventory(Items.Lantern))
                            {
                                if (_advHelper.GetItemState(Items.Lantern, ItemState.LanternIsOn) == 0)
                                    _responseBuilder.AddToResponse("The lamp is already off.");
                                else
                                {
                                    _advHelper.SetItemState(Items.Lantern, ItemState.LanternIsOn, 0);
                                    _responseBuilder.AddToResponse(MsgMnemonic.LampIsNowOff);
                                }
                            }
                            else
                            {
                                _responseBuilder.AddToResponse(MsgMnemonic.InvNotCarrying);
                            }
                        }
                        else
                        {
                            _responseBuilder.AddToResponse($"You can't {actionStr} the {itemStr}.");
                        }
                    }
                    else
                    {
                        _log.LogError($"Invalid action {actionStr} - expected 'On'or 'Off'");
                        _responseBuilder.AddToResponse(MsgMnemonic.VocabDontUnderstand);
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
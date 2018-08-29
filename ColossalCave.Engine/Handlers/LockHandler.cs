using System;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.Handlers
{
    public class LockHandler : BaseHandler, ILockHandler
    {
        public LockHandler(ILogger<LockHandler> log,
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
            _log.LogInformation("Handling lock-unlock");

            var actionStr = _advHelper.GetParameterValue("actions-lock");
            if (!Enum.TryParse<Actions>(actionStr, true, out var action))
            {
                _log.LogError($"Unknown action: {actionStr}");
                _responseBuilder.AddToResponse(MsgMnemonic.VocabDontUnderstand);
                return;
            }

            if (action != Actions.Lock && action != Actions.Unlock)
            {
                _log.LogError($"Invalid action {actionStr} - expected 'Lock' or 'Unlock'");
                _responseBuilder.AddToResponse(MsgMnemonic.VocabDontUnderstand);
                return;
            }

            var itemStr = _advHelper.GetParameterValue("items-moveable");
            if (itemStr == null)
                itemStr = _advHelper.GetParameterValue("treasures");
            if (itemStr == null)
                itemStr = _advHelper.GetParameterValue("items-fixed");
            if (!Enum.TryParse(itemStr, true, out Items targetItem))
            {
                _log.LogError($"Unknown item: {itemStr}");
                _responseBuilder.AddToResponse(MsgMnemonic.VocabDontUnderstand);
                return;
            }

            _log?.LogInformation($"Target item is {targetItem}");
            if (targetItem != Items.Grate)
            {
                _responseBuilder.AddToResponse($"You can't {actionStr} the {itemStr}.");
            }

            if (!_advHelper.IsItemAtCurrentLocation(Items.Grate))
            {
                _responseBuilder.AddToResponse(MsgMnemonic.ItemNotHere);
                return;
            }

            var hasKey = _advHelper.IsItemInInventory(Items.Keys);
            var grateStatus = _advHelper.GetItemState(Items.Grate, ItemState.GrateStatus);

            if (action == Actions.Unlock)
            {
                if (grateStatus != 0)
                {
                    _responseBuilder.AddToResponse("The grate is already unlocked.");
                }
                else
                {
                    if (hasKey)
                    {
                        _advHelper.SetItemState(Items.Grate, ItemState.GrateStatus, 1);
                        _responseBuilder.AddToResponse(MsgMnemonic.GrateNowUnlocked);
                    }
                    else
                    {
                        _responseBuilder.AddToResponse(MsgMnemonic.LockNoKeys);
                    }
                }
            }
            else if (action == Actions.Lock)
            {
                if (grateStatus == 0)
                {
                    _responseBuilder.AddToResponse("The grate is already locked.");
                }
                else if (grateStatus == 2)
                {
                    _responseBuilder.AddToResponse("You can't lock the grate when it is open.");
                }
                else if (grateStatus == 1)
                {
                    if (hasKey)
                    {
                        _advHelper.SetItemState(Items.Grate, ItemState.GrateStatus, 0);
                        _responseBuilder.AddToResponse(MsgMnemonic.GrateNowLocked);
                    }
                    else
                    {
                        _responseBuilder.AddToResponse(MsgMnemonic.LockNoKeys);
                    }
                }
            }
        



            //    if (Enum.TryParse<Actions>(actionStr, true, out var action))
            //{
            //    var itemStr = _advHelper.GetParameterValue("items-moveable");
            //    if (itemStr == null)
            //        itemStr = _advHelper.GetParameterValue("treasures");
            //    if (itemStr == null)
            //        itemStr = _advHelper.GetParameterValue("items-fixed");
            //    if (Enum.TryParse(itemStr, true, out Items targetItem))
            //    {
            //        _log?.LogInformation($"Target item is {targetItem}");
            //        var hasKey = _advHelper.IsItemInInventory(Items.Keys);
            //        if (action == Actions.Unlock)
            //        {
            //            if (targetItem == Items.Grate)
            //            {
            //                if (_advHelper.IsItemAtCurrentLocation(Items.Grate))
            //                {
            //                    if (_advHelper.GetItemState(Items.Grate, ItemState.GrateStatus) != 0)
            //                    {
            //                        _responseBuilder.AddToResponse("The grate is already unlocked.");
            //                    }
            //                    else
            //                    {
            //                        if (hasKey)
            //                        {
            //                            _advHelper.SetItemState(Items.Grate, ItemState.GrateStatus, 1);
            //                            _responseBuilder.AddToResponse(MsgMnemonic.GrateNowUnlocked);
            //                        }
            //                        else
            //                        {
            //                            _responseBuilder.AddToResponse(MsgMnemonic.LockNoKeys);
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    _responseBuilder.AddToResponse(MsgMnemonic.ItemNotHere);
            //                }
            //            }
            //            else
            //            {
            //                _responseBuilder.AddToResponse($"You can't {actionStr} the {itemStr}.");
            //            }
            //        }
            //        else if (action == Actions.Lock)
            //        {
            //            if (targetItem == Items.Grate)
            //            {
            //                if (_advHelper.IsItemAtCurrentLocation(Items.Grate))
            //                {
            //                    var status = _advHelper.GetItemState(Items.Grate, ItemState.GrateStatus);
            //                    if (status == 0)
            //                    {
            //                        _responseBuilder.AddToResponse("The grate is already locked.");
            //                    }
            //                    else if (status == 2)
            //                    {
            //                        _responseBuilder.AddToResponse("You can't lock the grate when it is open.");
            //                    }
            //                    else if (status == 1)
            //                    {
            //                        if (hasKey)
            //                        {
            //                            _advHelper.SetItemState(Items.Grate, ItemState.GrateStatus, 0);
            //                            _responseBuilder.AddToResponse(MsgMnemonic.GrateNowLocked);
            //                        }
            //                        else
            //                        {
            //                            _responseBuilder.AddToResponse(MsgMnemonic.LockNoKeys);
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    _responseBuilder.AddToResponse(MsgMnemonic.ItemNotHere);
            //                }
            //            }
            //            else
            //            {
            //                _responseBuilder.AddToResponse($"You can't {actionStr} the {itemStr}.");
            //            }
            //        }
            //        else
            //        {
            //            _log.LogError($"Invalid action {actionStr} - expected 'Lock' or 'Unlock'");
            //            _responseBuilder.AddToResponse(MsgMnemonic.VocabDontUnderstand);
            //        }
            //    }
            //    else
            //    {
            //        _log.LogError($"Unknown item: {itemStr}");
            //        _responseBuilder.AddToResponse(MsgMnemonic.VocabDontUnderstand);
            //    }

            //}
            //else
            //{
            //    _log.LogError($"Unknown action: {actionStr}");
            //    _responseBuilder.AddToResponse(MsgMnemonic.VocabDontUnderstand);
            //}
        }
    }
}

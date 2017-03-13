using System;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.ActionHandlers
{
    public class ControlHandler : BaseHandler, IControlHandler
    {
        public ControlHandler(ILogger<ControlHandler> log,
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
            _log.LogInformation("Handling Control");

            var controlStr = _advHelper.GetParameterValue("controlwords");
            if (Enum.TryParse(controlStr, true, out ControlWords controlWord))
            {
                if (controlWord == ControlWords.Inventory)
                    EnumerateInventory();
                else
                    _responseBuilder.PrefixResponse(MsgMnemonic.VocabDontKnowWord, 1);
            }
            else
            {
                _responseBuilder.PrefixResponse(MsgMnemonic.VocabDontUnderstand, 1);
            }
        }
        public void EnumerateInventory()
        {
            if (_advHelper.IsInventoryEmpty)
            {
                _responseBuilder.AddToResponse(MsgMnemonic.InvNotCarryingAnything);
                return;
            }

            var items = _advHelper.GetInventory();
            if (items.Count > 0)
                _responseBuilder.AddToResponse(MsgMnemonic.InvListHeader, 1);
            foreach (var item in items)
            {
                _responseBuilder.AddToResponse(item.ShortDescription + ". ", 1);
            }
        }
    }
}

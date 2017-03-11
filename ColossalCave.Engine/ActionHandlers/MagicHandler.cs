using System;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.ActionHandlers
{
    public class MagicHandler : BaseHandler, IMagicHandler
    {
        public MagicHandler(ILogger<MagicHandler> log,
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
            _log.LogInformation("Handling Magic");

            var theMap = _locationProvider.Map;
            var curLoc = _advContext.CurrentLocation;
            _log.LogInformation($"Current location: {curLoc}");
            var newLoc = curLoc;

            var magicStr = _advContext.Parameters["magicwords"];
            if (Enum.TryParse(magicStr, true, out Magicwords magic))
            {
                if (magic == Magicwords.XYZZY &&
                    _advContext.Flags.HasFlag(Flags.KnowsXYZZY) &&
                    (curLoc.Id == 3 || curLoc.Id == 11))
                {
                    newLoc = curLoc.Id == 3 ? theMap[11] : theMap[3];
                }
                else if (magic == Magicwords.Plugh &&
                    _advContext.Flags.HasFlag(Flags.KnowsPlugh) &&
                    (curLoc.Id == 3 || curLoc.Id == 33))
                {
                    newLoc = curLoc.Id == 3 ? theMap[33] : theMap[3];
                }
                else if (magic == Magicwords.Plover &&
                    _advContext.Flags.HasFlag(Flags.KnowsPlover) &&
                    (curLoc.Id == 100 || curLoc.Id == 33))
                {
                    newLoc = curLoc.Id == 100 ? theMap[33] : theMap[100];
                }
                //else if (magic == Magicword.fee)
                else
                {
                    _responseBuilder.PrefixResponse(Mnemonic.NothingHappens, 1);
                }
            }
            else
                _responseBuilder.PrefixResponse(Mnemonic.DontUnderstand, 1);

            _log.LogInformation($"New location: {newLoc}");

            _mapHelper.Move(curLoc, newLoc);
        }
    }
}

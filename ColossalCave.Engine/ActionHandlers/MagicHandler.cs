using System;
using ColossalCave.Engine.Enumerations;
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
                    _advContext.Flags.HasFlag(AdventureContextFlags.KnowsXYZZY) &&
                    (curLoc.Mnemonic == LocMnemonics.House || curLoc.Mnemonic == LocMnemonics.Debris))
                {
                    newLoc = curLoc.Mnemonic == LocMnemonics.House 
                        ? theMap[(int)LocMnemonics.Debris] 
                        : theMap[(int)LocMnemonics.House];
                }
                else if (magic == Magicwords.Plugh &&
                    _advContext.Flags.HasFlag(AdventureContextFlags.KnowsPlugh) &&
                    (curLoc.Mnemonic == LocMnemonics.House || curLoc.Mnemonic == LocMnemonics.Y2))
                {
                    newLoc = curLoc.Mnemonic == LocMnemonics.House
                        ? theMap[(int)LocMnemonics.Y2]
                        : theMap[(int)LocMnemonics.House];
                }
                else if (magic == Magicwords.Plover &&
                    _advContext.Flags.HasFlag(AdventureContextFlags.KnowsPlover) &&
                    (curLoc.Mnemonic == LocMnemonics.Plover || curLoc.Mnemonic == LocMnemonics.Y2))
                {
                    newLoc = curLoc.Mnemonic == LocMnemonics.Plover
                        ? theMap[(int)LocMnemonics.Y2]
                        : theMap[(int)LocMnemonics.Plover];
                }
                //else if (magic == Magicword.fee)
                else
                {
                    _responseBuilder.PrefixResponse(MsgMnemonic.NothingHappens, 1);
                }
            }
            else
                _responseBuilder.PrefixResponse(MsgMnemonic.VocabDontUnderstand, 1);

            _log.LogInformation($"New location: {newLoc}");

            _mapHelper.Move(newLoc);
        }
    }
}

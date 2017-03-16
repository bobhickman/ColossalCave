using System;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.Handlers
{
    public class MagicHandler : BaseHandler, IMagicHandler
    {
        public MagicHandler(ILogger<MagicHandler> log,
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
            _log.LogInformation("Handling Magic");

            var curLoc = _advHelper.CurrentLocation;
            _log.LogInformation($"Current location: {curLoc}");
            var newLoc = curLoc;

            var magicStr = _advHelper.GetParameterValue("magicwords");
            if (Enum.TryParse(magicStr, true, out Magicwords magic))
            {
                var theMap = _locationProvider.Map;
                if (magic == Magicwords.XYZZY &&
                    _advHelper.HasFlag(AdventureContextFlags.KnowsXYZZY) &&
                    (curLoc.Mnemonic == LocMnemonics.House || curLoc.Mnemonic == LocMnemonics.Debris))
                {
                    newLoc = curLoc.Mnemonic == LocMnemonics.House 
                        ? theMap[(int)LocMnemonics.Debris] 
                        : theMap[(int)LocMnemonics.House];
                }
                else if (magic == Magicwords.Plugh &&
                    _advHelper.HasFlag(AdventureContextFlags.KnowsPlugh) &&
                    (curLoc.Mnemonic == LocMnemonics.House || curLoc.Mnemonic == LocMnemonics.Y2))
                {
                    newLoc = curLoc.Mnemonic == LocMnemonics.House
                        ? theMap[(int)LocMnemonics.Y2]
                        : theMap[(int)LocMnemonics.House];
                }
                else if (magic == Magicwords.Plover &&
                    _advHelper.HasFlag(AdventureContextFlags.KnowsPlover) &&
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

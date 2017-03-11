using System;
using System.Collections.Generic;
using System.Text;

namespace ColossalCave.Engine
{
    public enum Mnemonic
    {
        CantGoThatWay = 9,
        NeedDirection = 10,
        DontKnowWord = 12,
        DontUnderstand = 13,
        ImGame = 14,
        CantGiveMoreDetail = 15,
        PitchDark = 16,
        NothingHappens = 42,
        // Messages above 1000 are not part of original Advent
        TooFarAway = 1001
    }
}

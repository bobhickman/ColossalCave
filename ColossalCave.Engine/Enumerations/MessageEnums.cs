
namespace ColossalCave.Engine.Enumerations
{
    /// <summary>
    /// Handy mnemonics to get access to messages.
    /// </summary>
    public enum MsgMnemonic
    {
        MoveCantGoThatWay = 9,
        MoveNeedDirection = 10,
        VocabDontKnowWord = 12,
        VocabDontUnderstand = 13,
        ImGame = 14,
        CantGiveMoreDetail = 15,
        MovePitchDark = 16,
        InvAlreadyCarrying = 24,
        LockNothingHereWith = 28,
        InvNotCarrying = 29,
        LockNoKeys = 31,
        LockHasNone = 32,
        LockDontKnowHowTo = 33,
        LockAlreadyLocked = 34,
        GrateNowLocked = 35,
        GrateNowUnlocked = 36,
        LockAlreadyUnlocked = 37,
        NoLightSource = 38,
        LampIsNowOn = 39,
        LampIsNowOff = 40,
        NothingHappens = 42,
        OK = 54,
        HintGrate = 63,
        RubLamp = 75,
        NothingUnexpectedHappens = 76,
        InvFull = 92,
        GrateCantGoThrough = 93,
        InvNotCarryingAnything = 98,
        InvListHeader = 99,
        // Messages above 1000 are not part of original Advent
        MoveTooFarAway = 1001,
        ItemNotHere = 1002,
        GrateIsClosed = 1003
    }
}


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
        InvNotCarrying = 29,
        NothingHappens = 42,
        OK = 54,
        InvFull = 92,
        InvNotCarryingAnything = 98,
        InvListHeader = 99,
        // Messages above 1000 are not part of original Advent
        MoveTooFarAway = 1001,
        ItemNotHere = 1002 // Can't find equivalent in Advent
    }
}

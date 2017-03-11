using System;
using System.Collections.Generic;
using ColossalCave.Engine.AssetModels;

namespace ColossalCave.Engine
{
    public class AdventureContext
    {
        public string ContextId { get; set; }

        #region Inputs
        
        public string IntentName { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        #endregion

        #region Outputs

        public string SpeechResponse { get; set; }

        public string TextResponse { get; set; }

        #endregion

        #region Context for the current adventurer

        public Location CurrentLocation { get; set; }

        public Flags Flags { get; set; }

        #endregion

        public string GetParameterValue(string name)
        {
            var value = Parameters.ContainsKey(name) ? Parameters[name] : null;
            if (string.IsNullOrWhiteSpace(value))
                return null;
            return value;
        }
    }

    [Flags]
    public enum Flags
    {
        None = 0x00000000,
        KnowsXYZZY = 0x00000001,
        KnowsPlugh = 0x00000002,
        KnowsPlover = 0x00000004,
        KnowsFee = 0x00000008,
        GrateIsUnlocked = 0x00000010,
        GrateIsOpen = 0x00000020,
        Bit07 = 0x00000040,
        Bit08 = 0x00000080,
        Bit09 = 0x00000100,
        Bit10 = 0x00000200,
        Bit11 = 0x00000400,
        Bit12 = 0x00000800,
        Bit13 = 0x00001000,
        Bit14 = 0x00002000,
        Bit15 = 0x00004000,
        Bit16 = 0x00008000,
        Bit17 = 0x00010000,
        Bit18 = 0x00020000,
        Bit19 = 0x00040000,
        Bit20 = 0x00080000,
        Bit21 = 0x00100000,
        Bit22 = 0x00200000,
        Bit23 = 0x00400000,
        Bit24 = 0x00800000,
        Bit25 = 0x01000000,
        Bit26 = 0x02000000,
        Bit27 = 0x04000000,
        Bit28 = 0x08000000,
        Bit29 = 0x10000000,
        Bit30 = 0x20000000,
        Bit31 = 0x40000000
        //Bit32 = 0x80000000 // Not usable
    }
}

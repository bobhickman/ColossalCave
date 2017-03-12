
namespace ColossalCave.Engine.Enumerations
{
    /// <summary>
    /// Handy mnemonic access to locations.
    /// Enum names must match *both* of:
    ///   - Name from the locations.json data file
    ///   - Entity 'locations' from api.ai
    /// </summary>
    public enum LocMnemonics
    {
        Undefined = 0,
        Road = 1,
        Hill = 2,
        House = 3,
        Valley = 4,
        Forest = 5,
        //Forest = 6,
        Slit = 7,
        Depression = 8,
        Entrance = 9,
        Crawl = 10,
        Debris = 11,
        Pipes = 79
    }
}

namespace ColossalCave.Engine.Enumerations
{
    // All of these enums map 1-1 to api.ai entities.
    // Except for the locations entities, which are in the map.

    public enum Actions
    {
        Undefined = 0,
        Open,
        Take,
        Insert,
        Drop,
        Say,
        Nothing,
        Close,
        On,
        Off,
        Swing,
        Tame,
        Attack,
        Pour,
        Eat,
        Drink,
        Rub,
        Toss,
        Find,
        Feed,
        Fill,
        Blast,
        Read,
        Smash,
        Wake,
        Dig,
        Lock,
        Unlock
    }

    public enum ControlWords
    {
        Undefined = 0,
        Quit,
        Inventory,
        Score,
        Brief,
        Verbose,
        Credits,
        Help,
        Hint,
        Save,
        Information,
        Lost,
        Swear,
        Classic,
        Modern
    }

    public enum Directions
    {
        Undefined = 0,
        North,
        South,
        East,
        West,
        Northwest,
        Southwest,
        Southeast,
        NorthEast,
        Up,
        Down,
        In,
        Out,
        Away,
        Forward,
        Backward,
        Around,
        Over,
        Under
    }

    public enum Items
    {
        Undefined = 0,
        // 1-999 Moveable/Carryable items
        Keys = 1,
        Lantern = 2,
        Cage = 3,
        Rod = 4,
        Bird = 5,
        Pillow = 6,
        Magazine = 7,
        Food = 8,
        Bottle = 9,
        Batteries = 10,
        Water = 11,
        Oil = 12,
        Axe = 13,
        Knife = 14,
        Drawing = 15,
        Shards = 16,
        All = 17,
        // 1000-1999 Fixed items
        Grate = 1000,
        Steps = 1001,
        Door = 1002,
        Fissure = 1003,
        Tablet = 1004,
        Clam = 1005,
        Oyster = 1006,
        Plant = 1007,
        Stalactite = 1008,
        Drawings = 1009,
        Chasm = 1010,
        Message = 1011,
        Volcano = 1012,
        Machine = 1013,
        Carpet = 1014,
        Mirror = 1015,
        Figure = 1016,
        // 2000-2999 Treasures
        Nugget = 2000,
        Diamonds = 2001,
        Silver = 2002,
        Jewelry = 2003,
        Coins = 2004,
        Chest = 2005,
        Eggs = 2006,
        Trident = 2007,
        Vase = 2008,
        Emerald = 2009,
        Pyramid = 2010,
        Pearl = 2011,
        Rug = 2012,
        Spices = 2013,
        Chain = 2014
    }

    //public enum ItemsFixed
    //{
    //    Undefined = 0,
    //    Grate,
    //    Steps,
    //    Door,
    //    Fissure,
    //    Tablet,
    //    Clam,
    //    Oyster,
    //    Plant,
    //    Stalactite,
    //    Drawings,
    //    Chasm,
    //    Message,
    //    Volcano,
    //    Machine,
    //    Carpet,
    //    Mirror,
    //    Figure
    //}

    public enum Magicwords
    {
        Undefined = 0,
        XYZZY,
        Plugh,
        Plover,
        Fee,
        Fie,
        Foe,
        Foo,
        Fum,
        OpenSesame,
        Abracadabra,
        Shazam,
        HocusPocus
    }

    public enum Mobs
    {
        Undefined = 0,
        Pirate,
        Dragon,
        Troll,
        Dwarf,
        Bear
    }

    public enum Movements
    {
        Undefined = 0,
        Walk,
        Run,
        Crawl,
        Climb,
        Jump,
        Enter,
        Leave,
        Descend,
        Cross,
        Approach
    }

    //public enum Treasures
    //{
    //    Undefined = 0,
    //    Nugget,
    //    Diamonds,
    //    Silver,
    //    Jewelry,
    //    Coins,
    //    Chest,
    //    Eggs,
    //    Trident,
    //    Vase,
    //    Emerald,
    //    Pyramid,
    //    Pearl,
    //    Rug,
    //    Spices,
    //    Chain
    //}

    public enum Visuals
    {
        Undefined = 0,
        Look,
        Examine,
        Peer
    }
}

namespace ColossalCave.Engine
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
        Dig
    }

    public enum Commands
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
        Swear
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

    public enum ItemsMoveable
    {
        Undefined = 0,
        Keys,
        Lantern,
        Cage,
        Rod,
        Bird,
        Pillow,
        Magazine,
        Food,
        Bottle,
        Mirror,
        Batteries,
        Water,
        Oil,
        Axe,
        Knife,
        Drawing,
        Shards
    }

    public enum ItemsFixed
    {
        Undefined = 0,
        Grate,
        Steps,
        Door,
        Fissure,
        Tablet,
        Clam,
        Oyster,
        Plant,
        Stalactite,
        Drawings,
        Chasm,
        Message,
        Volcano,
        Machine,
        Carpet,
        Mirror,
        Figure
    }

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

    public enum Treasures
    {
        Undefined = 0,
        Nugget,
        Diamonds,
        Silver,
        Jewelry,
        Coins,
        Chest,
        Eggs,
        Trident,
        Vase,
        Emerald,
        Pyramid,
        Pearl,
        Rug,
        Spices,
        Chain
    }

    public enum Visuals
    {
        Undefined = 0,
        Look,
        Examine,
        Peer
    }
}

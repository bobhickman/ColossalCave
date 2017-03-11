namespace ColossalCave.Engine
{
    // All of these enums map 1-1 to api.ai entities.
    // Except for the locations entities, which are in the map.

    public enum Actions
    {
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
        Key,
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
        Pirate,
        Dragon,
        Troll,
        Dwarf,
        Bear
    }

    public enum Movements
    {
        Walk,
        Run,
        Crawl,
        Climb,
        Jump,
        Enter,
        Leave,
        Descend,
        Cross
    }

    public enum Treasures
    {
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
        Look,
        Examine,
        Peer
    }
}

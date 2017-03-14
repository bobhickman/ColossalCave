using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;

namespace ColossalCave.Engine.AssetProviders
{
    public class LocationProvider : ILocationProvider
    {
        private readonly ILogger _log;
        private readonly IResourceLoader _resourceLoader;

        private Dictionary<int, Location> _locations;

        public LocationProvider(ILogger<LocationProvider> log,
            IResourceLoader resourceLoader)
        {
            _log = log;
            _resourceLoader = resourceLoader;
        }

        public Dictionary<int,Location> Map
        {
            get
            {
                if (_locations == null)
                    LoadLocations();
                return _locations;
            }
        }

        public Location GetLocation(int id)
        {
            if (_locations == null)
                LoadLocations();
            return _locations.FirstOrDefault(l => l.Key == id).Value;
        }

        /// <summary>
        /// Loads the definition of every available location
        /// </summary>
        private void LoadLocations()
        {
            _log?.LogInformation("Loading location assets...");

#if true
            LoadFromJsonFile();
#else 
            LoadFromCode();
#endif
            ResolveMap();
        }

        /// <summary>
        /// Resolves the entire map
        /// </summary>
        private void ResolveMap()
        {
            _log?.LogInformation("Resolving the map...");
            foreach(var loc in _locations.Values)
            {
                // Resolve exits
                var uniqueExitId = 1;
                if (loc.Exits != null)
                {
                    foreach (var exit in loc.Exits.Values)
                    {
                        var tryGo = _locations.FirstOrDefault(l => l.Key == exit.Id).Value;
                        if (tryGo != null)
                            exit.GoesTo = tryGo;
                        else
                            _log?.LogError($"Location {exit.Id} is undefined.");
                        exit.Id = uniqueExitId++;

                        // Resolve random exits
                        if (exit.GoesToRandom != null)
                        {
                            foreach(var rand in exit.GoesToRandom)
                            {
                                var tryRand = _locations.FirstOrDefault(l => l.Key == rand.GoesTo.Id).Value;
                                if (tryRand != null)
                                    rand.GoesTo = tryRand;
                                else
                                    _log?.LogError($"Location {rand.GoesTo.Id} is undefined.");
                            }
                        }
                    }
                }

                // Resolve fast travel
                if (loc.FastTravel != null)
                {
                    var resolvedFastTravel = new List<Location>();
                    foreach (var fast in loc.FastTravel)
                    {
                        var tryFast = _locations.FirstOrDefault(l => l.Key == fast.Id).Value;
                        if (tryFast != null)
                            resolvedFastTravel.Add(tryFast);
                        else
                            _log?.LogError($"Location {fast.Id} is undefined.");
                    }
                    loc.FastTravel = resolvedFastTravel;
                }
            }
        }

        private void LoadFromJsonFile()
        {
            var json = _resourceLoader.LoadAsset("locations.json");
            _locations = JsonConvert.DeserializeObject<Dictionary<int, Location>>(json);
        }

        private void LoadFromCode()
        {
            _locations = new Dictionary<int, Location>
            {
                {
                    1, new Location
                    {
                        Id = 1,
                        Mnemonic = LocMnemonics.Road,
                        Name = "road",
                        ShortDescription = "You are at the end of the road.",
                        Description = "You are standing at the end of a road before a small brick building. Around you is a forest. A small stream flows out of the building and down a gully.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.North, new Exit(5) }, // Forest
                            { Directions.South, new Exit(4) }, // Valley
                            { Directions.East, new Exit(3) },  // Building
                            { Directions.West, new Exit(2) },  // Hill
                            { Directions.Up, new Exit(2) },    // Hill
                            { Directions.Down, new Exit(4) },  // Valley
                            { Directions.In, new Exit(3) }
                        },
                        FastTravel = new List<Location>
                        {
                            { new Location(2) },
                            { new Location(3) },
                            { new Location(4) },
                            { new Location(5) },
                            { new Location(6) }
                        },
                        IsLight = true
                    }
                },
                {
                    2, new Location
                    {
                        Id = 2,
                        Mnemonic = LocMnemonics.Hill,
                        Name = "hill",
                        ShortDescription = "You are on a hill in the road.",
                        Description = "You have walked up a hill, still in the forest. The road slopes back down the other side of the hill. There is a building in the distance.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.North, new Exit(1) },
                            { Directions.South, new Exit(5) },
                            { Directions.East, new Exit(1) },
                            { Directions.Down, new Exit(1) }
                        },
                        FastTravel = new List<Location>
                        {
                            { new Location(1) },
                            { new Location(5) }
                        },
                        IsLight = true
                    }
                },
                {
                    3, new Location
                    {
                        Id = 3,
                        Mnemonic = LocMnemonics.House,
                        Name = "house",
                        ShortDescription = "You are inside a well house.",
                        Description = "You are inside a building, a well house for a large spring.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.West, new Exit(1) },
                            { Directions.Out, new Exit(1) },
                            { Directions.Down, new Exit(79) }
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(79)
                        },
                        IsLight = true
                    }
                },
                {
                    4, new Location
                    {
                        Id = 4,
                        Mnemonic = LocMnemonics.Valley,
                        Name = "valley",
                        ShortDescription = "You are in a valley.",
                        Description = "You are in a valley in the forest beside a stream tumbling along a rocky bed.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.Up, new Exit(1) },
                            { Directions.North, new Exit(1) },
                            { Directions.West, new Exit(5) },
                            { Directions.East, new Exit(5) },
                            { Directions.South, new Exit(7) },
                            { Directions.Down, new Exit(7) }
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(1),
                            new Location(5),
                            new Location(8),

                        },
                        IsLight = true
                    }
                },
                {
                    5, new Location
                    {
                        Id = 5,
                        Mnemonic = LocMnemonics.Forest,
                        Name = "forest",
                        ShortDescription = "You are in the forest.",
                        Description = "You are in open forest, with a deep valley to one side.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.East, new Exit(4) },
                            { Directions.Down, new Exit(4) },
                            { Directions.West, new Exit(5) },
                            { Directions.South, new Exit(5) },
                            { Directions.North, new Exit(5)
                                {
                                    GoesToRandom = new List<RandomExit>
                                    {
                                        { new RandomExit { Probability = 50, GoesTo = new Location(5) } },
                                        { new RandomExit { Probability = 100, GoesTo = new Location(6) } }
                                    }
                                }
                            }
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(4),
                            new Location(5),
                            new Location(6)
                        },
                        IsLight = true
                    }
                },
                {
                    6, new Location
                    {
                        Id = 6,
                        Mnemonic = LocMnemonics.Forest2,
                        Name = "forest2",
                        ShortDescription = "You are in the forest.",
                        Description = "You are in open forest near both a valley and a road.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.North, new Exit(1) },
                            { Directions.East, new Exit(4) },
                            { Directions.West, new Exit(4) },
                            { Directions.Down, new Exit(4) },
                            { Directions.South, new Exit(5) },
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(1),
                            new Location(4),
                            new Location(5)
                        },
                        IsLight = true
                    }
                },
                {
                    7, new Location
                    {
                        Id = 7,
                        Mnemonic = LocMnemonics.Slit,
                        Name = "slit",
                        ShortDescription = "You are at a slit in the streambed.",
                        Description = "At your feet all the water of the stream splashes into a 2-inch slit in the rock. Downstream the streambed is bare rock.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.North, new Exit(4) },
                            { Directions.East, new Exit(5) },
                            { Directions.West, new Exit(5) },
                            { Directions.South, new Exit(8) },
                            { Directions.Up, new Exit(4) },
                            { Directions.Down, new Exit(8) },
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(1),
                            new Location(5),
                            new Location(8)
                        },
                        IsLight = true
                    }
                },
                {
                    8, new Location
                    {
                        Id = 8,
                        Mnemonic = LocMnemonics.Depression,
                        Name = "depression",
                        ShortDescription = "You are outside of a grate.",
                        Description = "You are in a 20-foot depression floored with bare dirt. Set into the dirt is a strong steel grate mounted in concrete. A dry streambed leads into the depression.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.North, new Exit(7) },
                            { Directions.East, new Exit(5) },
                            { Directions.West, new Exit(5) },
                            { Directions.South, new Exit(5) },
                            { Directions.Up, new Exit(7) },
                            { Directions.Down, new Exit(9) },
                            { Directions.In, new Exit(9) },
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(1),
                            new Location(5),
                            new Location(7)
                        },
                        IsLight = true
                    }
                },
                {
                    9, new Location
                    {
                        Id = 9,
                        Mnemonic = LocMnemonics.Entrance,
                        Name = "entrance",
                        ShortDescription = "You are below a grate.",
                        Description = "You are in a small chamber beneath a 3 by 3 steel grate to the surface. A low crawl over cobbles leads inward to the west.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.Up, new Exit(8) },
                            { Directions.Out, new Exit(8) },
                            { Directions.West, new Exit(10) },
                            { Directions.In, new Exit(10) },
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(10),
                            new Location(11),
                            new Location(14)
                        },
                        IsLight = true
                    }
                },
                {
                    10, new Location
                    {
                        Id = 10,
                        Mnemonic = LocMnemonics.Crawl,
                        Name = "crawl",
                        ShortDescription = "You are in a cobble crawl.",
                        Description = "You are crawling over cobbles in a low passage. There is a dim light at the east end of the passage.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.East, new Exit(9) },
                            { Directions.Out, new Exit(9) },
                            { Directions.West, new Exit(11) },
                            { Directions.In, new Exit(11) },
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(9),
                            new Location(11),
                            new Location(14)
                        },
                        IsLight = true
                    }
                },
                {
                    11, new Location
                    {
                        Id = 11,
                        Mnemonic = LocMnemonics.Debris,
                        Name = "debris",
                        ShortDescription = "You are in a room full of debris.",
//                        Description = "You are in a debris room filled with stuff washed in from the surface. A low wide passage with cobbles becomes plugged with mud and debris here, but an awkward canyon leads upward and west. A note on the wall says \"Magic word XYZZY\".",
                        Description = @"
You are in a debris room filled with stuff washed in from the surface. 
A low wide passage with cobbles becomes plugged with mud and debris here, but an awkward canyon leads upward and west. 
<break time='1s'/>
A note on the wall says 'Magic word <say-as interpret-as='characters'>XYZZY</say-as>'.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.East, new Exit(10) },
                            { Directions.West, new Exit(12) },
                            { Directions.Up, new Exit(12) },
                            { Directions.In, new Exit(12) },
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(8),
                            new Location(9),
                            new Location(10),
                            new Location(12),
                            new Location(14),
                        }
                    }
                },
                {
                    12, new Location
                    {
                        Id = 12,
                        Mnemonic = LocMnemonics.Canyon,
                        Name = "canyon",
                        ShortDescription = "You are in an awkward sloping east/west canyon.",
                        Description = "You are in an awkward sloping east/west canyon.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.East, new Exit(11) },
                            { Directions.Down, new Exit(11) },
                            { Directions.West, new Exit(13) },
                            { Directions.In, new Exit(13) },
                            { Directions.Up, new Exit(13) },
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(8),
                            new Location(9),
                            new Location(14)
                        }
                    }
                },
                {
                    13, new Location
                    {
                        Id = 13,
                        Mnemonic = LocMnemonics.Bird,
                        Name = "bird",
                        ShortDescription = "You are in the bird chamber.",
                        Description = "You are in a splendid chamber thirty feet high. The walls are frozen rivers of orange stone. An awkward canyon and a good passage exit from east and west sides of the chamber.",
                        Exits = new Dictionary<Directions, Exit>
                        {
                            { Directions.East, new Exit(12) },
                            { Directions.West, new Exit(14) },
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(8),
                            new Location(9),
                            new Location(11),
                            new Location(12),
                            new Location(14)
                        }
                    }
                },
                {
                    79, new Location
                    {
                        Id = 79,
                        Mnemonic = LocMnemonics.Pipes,
                        Name = "stream",
                        ShortDescription = "You are at sewer pipes. You can't get out this way.",
                        Description = "The stream flows out through a pair of 1 foot diameter sewer pipes. It would be advisable to use the exit.",
                        IsLight = true
                    }
                }
            };

            // Uncomment this to write out the entire unresolved map as json
            // Useful to seed the locations.json file.
            var json = JsonConvert.SerializeObject(_locations);
            File.WriteAllText(@"c:\temp\locations.json", json);
        }
    }
}

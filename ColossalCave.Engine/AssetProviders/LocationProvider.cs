using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
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

        private Dictionary<int, Location> _locations;

        public LocationProvider(ILogger<LocationProvider> log)
        {
            _log = log;
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

#if true // Load from json file
            var assembly = typeof(LocationProvider).GetTypeInfo().Assembly;
            string[] names = assembly.GetManifestResourceNames();
            var stream = assembly.GetManifestResourceStream("ColossalCave.Engine.Assets.locations.json");
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var json = reader.ReadToEnd();
                _locations = JsonConvert.DeserializeObject<Dictionary<int, Location>>(json);
            }
#else // hard-coded
            _locations = new Dictionary<int, Location>
            {
                {
                    1, new Location
                    {
                        Id = 1,
                        Name = "road",
                        ShortDescription = "at the end of the road",
                        Description = "You are standing at the end of a road before a small brick building. Around you is a forest. A small stream flows out of the building and down a gully.",
                        Exits = new Dictionary<Direction, Exit>
                        {
                            { Direction.North, new Exit(5) }, // Forest
                            { Direction.South, new Exit(4) }, // Valley
                            { Direction.East, new Exit(3) },  // Building
                            { Direction.West, new Exit(2) },  // Hill
                            { Direction.Up, new Exit(2) },    // Hill
                            { Direction.Down, new Exit(4) },  // Valley
                            { Direction.In, new Exit(3) }
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
                        Name = "hill",
                        ShortDescription = "on a hill in the road",
                        Description = "You have walked up a hill, still in the forest. The road slopes back down the other side of the hill. There is a building in the distance.",
                        Exits = new Dictionary<Direction, Exit>
                        {
                            { Direction.North, new Exit(1) },
                            { Direction.South, new Exit(5) },
                            { Direction.East, new Exit(1) },
                            { Direction.Down, new Exit(1) }
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
                        Name = "house",
                        ShortDescription = "inside a building",
                        Description = "You are inside a building, a well house for a large spring.",
                        Exits = new Dictionary<Direction, Exit>
                        {
                            { Direction.West, new Exit(1) },
                            { Direction.Out, new Exit(1) },
                            { Direction.Down, new Exit(79) },
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
                        Name = "valley",
                        ShortDescription = "in a valley",
                        Description = "You are in a valley in the forest beside a stream tumbling along a rocky bed.",
                        Exits = new Dictionary<Direction, Exit>
                        {
                            { Direction.Up, new Exit(1) },
                            { Direction.North, new Exit(1) },
                            { Direction.West, new Exit(5) },
                            { Direction.East, new Exit(5) },
                            { Direction.South, new Exit(7) },
                            { Direction.Down, new Exit(7) }
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
                        Name = "forest",
                        ShortDescription = "in the forest",
                        Description = "You are in open forest, with a deep valley to one side.",
                        Exits = new Dictionary<Direction, Exit>
                        {
                            { Direction.East, new Exit(4) },
                            { Direction.Down, new Exit(4) },
                            { Direction.West, new Exit(5) },
                            { Direction.South, new Exit(5) },
                            { Direction.North, new Exit(5)
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
                        Name = "forest",
                        ShortDescription = "in the forest",
                        Description = "You are in open forest near both a valley and a road.",
                        Exits = new Dictionary<Direction, Exit>
                        {
                            { Direction.North, new Exit(1) },
                            { Direction.East, new Exit(4) },
                            { Direction.West, new Exit(4) },
                            { Direction.Down, new Exit(4) },
                            { Direction.South, new Exit(5) },
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
                        Name = "slit",
                        ShortDescription = "at a slit in the streambed",
                        Description = "At your feet all the water of the stream splashes into a 2-inch slit in the rock. Downstream the streambed is bare rock.",
                        Exits = new Dictionary<Direction, Exit>
                        {
                            { Direction.North, new Exit(4) },
                            { Direction.East, new Exit(5) },
                            { Direction.West, new Exit(5) },
                            { Direction.South, new Exit(8) },
                            { Direction.Up, new Exit(4) },
                            { Direction.Down, new Exit(8) },
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
                    8, new Location
                    {
                        Id = 8,
                        Name = "depression",
                        ShortDescription = "outside of a grate",
                        Description = "You are in a 20-foot depression floored with bare dirt. Set into the dirt is a strong steel grate mounted in concrete. A dry streambed leads into the depression.",
                        Exits = new Dictionary<Direction, Exit>
                        {
                            { Direction.North, new Exit(7) },
                            { Direction.East, new Exit(5) },
                            { Direction.West, new Exit(5) },
                            { Direction.South, new Exit(5) },
                            { Direction.Up, new Exit(7) },
                            { Direction.Down, new Exit(9) },
                            { Direction.In, new Exit(9) },
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(1),
                            new Location(5),
                            new Location(7),
                        },
                        IsLight = true
                    }
                },
                {
                    9, new Location
                    {
                        Id = 9,
                        Name = "entrance",
                        ShortDescription = "below a grate",
                        Description = "You are in a small chamber beneath a 3 by 3 steel grate to the surface. A low crawl over cobbles leads inward to the west.",
                        Exits = new Dictionary<Direction, Exit>
                        {
                            { Direction.Up, new Exit(8) },
                            { Direction.Out, new Exit(8) },
                            { Direction.West, new Exit(10) },
                            { Direction.In, new Exit(10) },
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(10),
                            new Location(11),
                            new Location(14),
                        },
                        IsLight = true
                    }
                },
                {
                    10, new Location
                    {
                        Id = 10,
                        Name = "crawl",
                        ShortDescription = "in a cobble crawl",
                        Description = "You are crawling over cobbles in a low passage. There is a dim light at the east end of the passage.",
                        Exits = new Dictionary<Direction, Exit>
                        {
                            { Direction.East, new Exit(9) },
                            { Direction.Out, new Exit(9) },
                            { Direction.West, new Exit(11) },
                            { Direction.In, new Exit(11) },
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(9),
                            new Location(11),
                            new Location(14),
                        },
                        IsLight = true
                    }
                },
                {
                    11, new Location
                    {
                        Id = 11,
                        Name = "debris",
                        ShortDescription = "in the debris room",
//                        Description = "You are in a debris room filled with stuff washed in from the surface. A low wide passage with cobbles becomes plugged with mud and debris here, but an awkward canyon leads upward and west. A note on the wall says \"Magic word XYZZY\".",
                        Description = @"
You are in a debris room filled with stuff washed in from the surface. 
A low wide passage with cobbles becomes plugged with mud and debris here, but an awkward canyon leads upward and west. 
<break time='1s'/>
A note on the wall says 'Magic word <say-as interpret-as='characters'>XYZZY</say-as>'.",
                        Exits = new Dictionary<Direction, Exit>
                        {
                            { Direction.East, new Exit(10) },
                            { Direction.West, new Exit(12) },
                            { Direction.Up, new Exit(12) },
                            { Direction.In, new Exit(12) },
                        },
                        FastTravel = new List<Location>
                        {
                            new Location(8),
                            new Location(9),
                            new Location(10),
                            new Location(12),
                            new Location(14),
                        },
                    }
                },
                {
                    79, new Location
                    {
                        Id = 79,
                        Name = "stream",
                        ShortDescription = "at sewer pipes",
                        Description = "The stream flows out through a pair of 1 foot diameter sewer pipes. It would be advisable to use the exit."
                    }
                },
            };

            // Uncomment this to write out the entire unresolved map as json
            // Useful to seed the locations.json file.
            // Make sure you redirect output to a file to capture it.
            var json = JsonConvert.SerializeObject(_locations);
            System.Console.WriteLine(json);
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
    }
}

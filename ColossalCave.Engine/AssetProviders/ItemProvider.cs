using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ColossalCave.Engine.AssetProviders
{
    public class ItemProvider : IItemProvider
    {
        private readonly ILogger _log;
        private readonly ILocationProvider _locationProvider;

        private List<Item> _items { get; set; }

        public ItemProvider(ILogger<ItemProvider> log,
            ILocationProvider locationProvider)
        {
            _log = log;
            _locationProvider = locationProvider;
        }

        public List<Item> Items
        {
            get
            {
                if (_items == null)
                    LoadItems();
                return _items;
            }
        }

        public Item GetItem(int id)
        {
            if (_items == null)
                LoadItems();
            return _items.FirstOrDefault(i => i.Id == id);
        }

        public Item GetItem(ItemsMoveable item)
        {
            if (_items == null)
                LoadItems();
            return _items.FirstOrDefault(i => i.ItemEnum == item);
        }

        private void LoadItems()
        {
            _log?.LogInformation("Loading item assets...");

#if false // Load from json file
            var assembly = typeof(LocationProvider).GetTypeInfo().Assembly;
            string[] names = assembly.GetManifestResourceNames();
            var stream = assembly.GetManifestResourceStream("ColossalCave.Engine.Assets.items.json");
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var json = reader.ReadToEnd();
                _items = JsonConvert.DeserializeObject<List<Item>>(json);
            }
#else // hard-coded
            _items = new List<Item>()
            {
                new Item
                {
                    Id = (int)ItemsMoveable.Keys,
                    ItemEnum = ItemsMoveable.Keys,
                    Name = ItemsMoveable.Keys.ToString(),
                    ShortDescription = "Keys",
                    Description = "set of keys",
                    InitialLocationId = 3,
                    FoundDescriptions = new List<Tuple<AdventureContextFlags, bool, string>>
                    {
                        new Tuple<AdventureContextFlags, bool, string>(AdventureContextFlags.None, false, "There are some keys on the ground here."),
                    }
                },
                new Item
                {
                    Id = (int)ItemsMoveable.Lantern,
                    ItemEnum = ItemsMoveable.Lantern,
                    Name = ItemsMoveable.Lantern.ToString(),
                    ShortDescription = "Brass lantern",
                    Description = "shiny brass lamp",
                    InitialLocationId = 3,
                    FoundDescriptions = new List<Tuple<AdventureContextFlags, bool, string>>
                    {
                        new Tuple<AdventureContextFlags, bool, string>(AdventureContextFlags.LanternIsOn, false, "There is a shiny brass lamp nearby."),
                        new Tuple<AdventureContextFlags, bool, string>(AdventureContextFlags.LanternIsOn, true, "There is a lamp shining nearby.")
                    }
                },
                new Item
                {
                    Id = (int)ItemsMoveable.Food,
                    ItemEnum = ItemsMoveable.Food,
                    Name = ItemsMoveable.Food.ToString(),
                    ShortDescription = "Food",
                    Description = "tasty food",
                    InitialLocationId = 3,
                    FoundDescriptions = new List<Tuple<AdventureContextFlags, bool, string>>
                    {
                        new Tuple<AdventureContextFlags, bool, string>(AdventureContextFlags.None, false, "There is food here."),
                    }
                },
                new Item
                {
                    Id = (int)ItemsMoveable.Cage,
                    ItemEnum = ItemsMoveable.Cage,
                    Name = ItemsMoveable.Cage.ToString(),
                    ShortDescription = "Wicker cage",
                    Description = "small wicker cage",
                    InitialLocationId = 10,
                    FoundDescriptions = new List<Tuple<AdventureContextFlags, bool, string>>
                    {
                        new Tuple<AdventureContextFlags, bool, string>(AdventureContextFlags.None, false, "There is a small wicker cage discarded nearby."),
                    }
                },
                new Item
                {
                    Id = (int)ItemsMoveable.Rod,
                    ItemEnum = ItemsMoveable.Rod,
                    Name = ItemsMoveable.Rod.ToString(),
                    ShortDescription = "Black rod",
                    Description = "three foot black rod",
                    InitialLocationId = 11,
                    FoundDescriptions = new List<Tuple<AdventureContextFlags, bool, string>>
                    {
                        new Tuple<AdventureContextFlags, bool, string>(AdventureContextFlags.RodIsMarked, false, "A three foot black rod with a rusty star on an end lies nearby."),
                        new Tuple<AdventureContextFlags, bool, string>(AdventureContextFlags.RodIsMarked, true, "A three foot black rod with a rusty mark on an end lies nearby.")
                    }
                }
            };

            // Uncomment this to write out the entire unresolved item dictionary as json
            // Useful to seed the items.json file.
            // Make sure you redirect output to a file to capture it.
            var json = JsonConvert.SerializeObject(_items);
            Console.WriteLine(json);
#endif
            //ResolveItems();
        }
        //private void ResolveItems()
        //{
        //    _log?.LogInformation("Resolving Items...");
        //    foreach (var item in _items)
        //    {
        //        // TODO: Need LocationProvider to resolve
        //        var tryLoc = _locationProvider.GetLocation(item.InitialLocation.Id);
        //        if (tryLoc != null)
        //            item.InitialLocation = tryLoc;
        //        else
        //            _log?.LogError($"Location {item.InitialLocation.Id} is undefined.");
        //    }
        //}
    }
}

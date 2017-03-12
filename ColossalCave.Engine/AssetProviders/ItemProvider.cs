using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalCave.Engine.AssetModels;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Interfaces;
using ColossalCave.Engine.Utilities;
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
                new Item // 1
                {
                    Id = (int)ItemsMoveable.Keys,
                    ItemEnum = ItemsMoveable.Keys,
                    Name = ItemsMoveable.Keys.ToString(),
                    ShortDescription = "A set of keys",
                    Description = "It's a large metal ring with a bunch of keys on it.",
                    InitialLocationId = 3,
                    FoundDescriptions = new List<Tuple<ItemStateValuePair, string>>
                    {
                        new Tuple<ItemStateValuePair, string>(null, "There are some keys on the ground here."),
                    }
                },
                new Item // 2
                {
                    Id = (int)ItemsMoveable.Lantern,
                    ItemEnum = ItemsMoveable.Lantern,
                    Name = ItemsMoveable.Lantern.ToString(),
                    ShortDescription = "A shiny brass lantern",
                    Description = "The lamp is brass and highly polished. There is a switch on it labeled 'On/Off'.",
                    InitialLocationId = 3,
                    DefaultStates = new List<ItemStateValuePair>
                    {
                        new ItemStateValuePair(ItemState.LanternIsOn, 1)
                    },
                    FoundDescriptions = new List<Tuple<ItemStateValuePair, string>>
                    {
                        new Tuple<ItemStateValuePair, string>(new ItemStateValuePair(ItemState.LanternIsOn, 0), "There is a shiny brass lamp nearby."),
                        new Tuple<ItemStateValuePair, string>(new ItemStateValuePair(ItemState.LanternIsOn, 1), "There is a lamp shining nearby.")
                    },
                    ExamineDescriptions = new List<Tuple<ItemStateValuePair, string>>
                    {
                        new Tuple<ItemStateValuePair, string>(new ItemStateValuePair(ItemState.LanternIsOn, 0), "The lamp is brass and highly polished. There is a switch on it labeled 'On/Off', which is currently in the 'Off' position."),
                        new Tuple<ItemStateValuePair, string>(new ItemStateValuePair(ItemState.LanternIsOn, 1), "The lamp is brass and highly polished. There is a switch on it labeled 'On/Off', which is currently in the 'On' position.")
                    }
                },
                new Item // 3
                {
                    Id = (int)ItemsMoveable.Cage,
                    ItemEnum = ItemsMoveable.Cage,
                    Name = ItemsMoveable.Cage.ToString(),
                    ShortDescription = "A small wicker cage",
                    Description = "The cage appears to be hand-woven wicker. It has a small door on the side.",
                    InitialLocationId = 10,
                    FoundDescriptions = new List<Tuple<ItemStateValuePair, string>>
                    {
                        new Tuple<ItemStateValuePair, string>(null, "There is a small wicker cage discarded nearby."),
                    }
                },
                new Item // 4
                {
                    Id = (int)ItemsMoveable.Rod,
                    ItemEnum = ItemsMoveable.Rod,
                    Name = ItemsMoveable.Rod.ToString(),
                    ShortDescription = "A three foot black rod",
                    Description = "The rod is about three feet long and black.",
                    InitialLocationId = 11,
                    DefaultStates = new List<ItemStateValuePair>
                    {
                        new ItemStateValuePair(ItemState.RodIsMarked, 0)
                    },
                    FoundDescriptions = new List<Tuple<ItemStateValuePair, string>>
                    {
                        new Tuple<ItemStateValuePair, string>(new ItemStateValuePair(ItemState.RodIsMarked, 0), "A three foot black rod with a rusty star on an end lies nearby."),
                        new Tuple<ItemStateValuePair, string>(new ItemStateValuePair(ItemState.RodIsMarked, 1), "A three foot black rod with a rusty mark on an end lies nearby.")
                    },
                    ExamineDescriptions = new List<Tuple<ItemStateValuePair, string>>
                    {
                        new Tuple<ItemStateValuePair, string>(new ItemStateValuePair(ItemState.RodIsMarked, 0), "The rod is about three feet long and black. It has a rusty star attached at one end."),
                        new Tuple<ItemStateValuePair, string>(new ItemStateValuePair(ItemState.RodIsMarked, 1), "The rod is about three feet long and black. It has a rusty mark on one end.")
                    }
                },
                new Item // 8
                {
                    Id = (int)ItemsMoveable.Food,
                    ItemEnum = ItemsMoveable.Food,
                    Name = ItemsMoveable.Food.ToString(),
                    ShortDescription = "Some tasty food",
                    Description = "It appears to be some sort of fruit and nut based nutritional bar.",
                    InitialLocationId = 3,
                    FoundDescriptions = new List<Tuple<ItemStateValuePair, string>>
                    {
                        new Tuple<ItemStateValuePair, string>(null, "There is food here."),
                    }
                },
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

        public string GetItemFoundDescription(Item item, List<ItemStateValuePair> states)
        {
            if (item.ItemEnum == ItemsMoveable.Lantern)
            {
                return GetSingleFoundDescription(item, states, ItemState.LanternIsOn);
            }
            else if (item.ItemEnum == ItemsMoveable.Rod)
            {
                return GetSingleFoundDescription(item, states, ItemState.RodIsMarked);
            }
            return item.Description;
        }

        public string GetItemExamineDescription(Item item, List<ItemStateValuePair> states)
        {
            if (item.ItemEnum == ItemsMoveable.Lantern)
            {
                return GetSingleExamineDescription(item, states, ItemState.LanternIsOn);
            }
            else if (item.ItemEnum == ItemsMoveable.Rod)
            {
                return GetSingleExamineDescription(item, states, ItemState.RodIsMarked);
            }
            return item.Description;
        }

        private string GetSingleFoundDescription(Item item,
            List<ItemStateValuePair> states, ItemState itemStateName)
        {
            var state = states.Find(p => p.ItemStateName == itemStateName).Value;
            return item.FoundDescriptions
                .Where(t => t.Item1.ItemStateName == itemStateName && t.Item1.Value == state)
                .First().Item2;
        }

        private string GetSingleExamineDescription(Item item,
            List<ItemStateValuePair> states, ItemState itemStateName)
        {
            var state = states.Find(p => p.ItemStateName == itemStateName).Value;
            return item.ExamineDescriptions
                .Where(t => t.Item1.ItemStateName == itemStateName && t.Item1.Value == state)
                .First().Item2;
        }

    }
}

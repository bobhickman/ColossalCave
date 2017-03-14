using System;
using ColossalCave.Engine.AssetProviders;
using ColossalCave.Engine;
using ColossalCave.Engine.Enumerations;
using ColossalCave.Engine.Utilities;

namespace ColossalCave.ConsoleRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var loader = new ResourceLoader();

            Console.WriteLine("*** MESSAGES *****************************************************\n");
            var mp = new MessageProvider(null, loader);
            foreach (var e in Enum.GetValues(typeof(MsgMnemonic)))
            {
                Console.WriteLine($"{e}: {mp.GetMessage((int)e).Speech}");
                Console.WriteLine($"{e}: {mp.GetMessage((int)e).Text}");
                Console.WriteLine();
            }

            Console.WriteLine("\n\n*** LOCATIONS ****************************************************\n");
            var lp = new LocationProvider(null, loader);
            foreach (var l in lp.Map)
                Console.WriteLine($"{l.Value.Id}: {l.Value.ShortDescription}");
            //var l = lp.GetLocation(2);
            //Console.WriteLine(l.Description);

            Console.WriteLine("\n\n*** ITEMS ********************************************************");
            var ip = new ItemProvider(null, lp, loader);
            //var i = ip.GetItem(ItemsMoveable.Keys);
            //Console.WriteLine(i.Description);
            foreach (var item in ip.Items)
            {
                Console.WriteLine($"\n{item}");
                foreach (var desc in item.FoundDescriptions)
                    Console.WriteLine($"{desc.Item1} - {desc.Item2}");
            }

            var context = new AdventureContext(ip, lp);

            Console.ReadLine();
        }
    }
}

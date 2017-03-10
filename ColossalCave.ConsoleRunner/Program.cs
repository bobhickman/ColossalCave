using System;
using ColossalCave.Engine.AssetProviders;
using ColossalCave.Engine.AssetModels;

namespace ColossalCave.ConsoleRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //ModelBuilder.LoadLocations();
            //var json = ModelBuilder.MakeJson();
            //Console.WriteLine(json);

            //var mp = new MessageProvider();
            //var m = mp.GetMessage(16);
            //Console.WriteLine(m.Speech);
            //Console.WriteLine(m.Text);

            var lp = new LocationProvider(null);
            var l = lp.GetLocation(2);
            //Console.WriteLine(l.Description);

            Console.ReadLine();
        }
    }
}

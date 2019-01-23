using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CoreCMS;
using MongoDB.Driver;

namespace ConsoleTestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await StartTestingRoutine();

            Console.ReadLine();
        }

        static async Task StartTestingRoutine()
        {
            const int fillAmmount = 2_500;

            var sw = new Stopwatch();
            sw.Start();
            await Cms.ContentSystem.ClearAsync();
            sw.Stop();
            Console.WriteLine($"Clenead all db entries in {sw.ElapsedMilliseconds} ms");
            sw = new Stopwatch(); 
            sw.Start();
            FillDatabase(fillAmmount);
            sw.Stop();
            Console.WriteLine($"Added all car entries in {sw.ElapsedMilliseconds} ms");

            sw = new Stopwatch();
            sw.Start();
            var allContents = Cms.ContentSystem.GetPage(0, fillAmmount);
            sw.Stop();
            Console.WriteLine($"Found all contents in {sw.ElapsedMilliseconds} ms");

            sw = new Stopwatch();
            var randomItem = allContents[new Random().Next(0, allContents.Length)];
            sw.Start();
            var randomItemFromDb = Cms.ContentSystem.GetById(randomItem.Id);
            sw.Stop();
            Console.WriteLine($"Found a random item in {sw.ElapsedMilliseconds} ms");
        }

        static void FillDatabase(int toSave)
        {
            const int reportAt = 10_000;
            int counter = 0;
            var cacheList = new Car[toSave];
            for (int i = 0; i < toSave; i++)
            {
                counter++;
                var newCar = new Car();
                cacheList[i] = newCar;

                if (counter >= reportAt)
                {
                    counter = 0;
                }
            }
            Console.WriteLine($"Cache created... saving into the DB...");
            Cms.ContentSystem.Collection.InsertMany(cacheList);
            Console.WriteLine($"{toSave} cars saved.");
        }
    }
}

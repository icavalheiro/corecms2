using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CoreCMS;
using MongoDB.Driver;
using Newtonsoft.Json;

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

            //cleaning database
            var sw = new Stopwatch();
            sw.Start();
            await Cms.ContentSystem.ClearAsync();
            sw.Stop();
            Console.WriteLine($"Clenead all db entries in {sw.ElapsedMilliseconds} ms");

            //generate new values and at it to the database
            sw = new Stopwatch(); 
            sw.Start();
            await FillDatabase(fillAmmount);
            sw.Stop();
            Console.WriteLine($"Added all car entries in {sw.ElapsedMilliseconds} ms");

            //get all contents
            sw = new Stopwatch();
            sw.Start();
            var allContents = Cms.ContentSystem.GetPage(0, fillAmmount);
            sw.Stop();
            Console.WriteLine($"Found all contents in {sw.ElapsedMilliseconds} ms");

            //get random content
            sw = new Stopwatch();
            var randomItem = allContents[new Random().Next(0, allContents.Length)];
            sw.Start();
            var randomItemFromDb = Cms.ContentSystem.GetById(randomItem.Id);
            sw.Stop();
            Console.WriteLine($"Found a random item in {sw.ElapsedMilliseconds} ms");

            //serialization tests
            var randomItemSerialized = JsonConvert.SerializeObject(new SerializableContent(randomItemFromDb));
            Console.WriteLine($"Serialized item: {randomItemSerialized}");
            var deserializedItem = JsonConvert.DeserializeObject<SerializableContent>(randomItemSerialized);
            var deserializedContent = deserializedItem.Deserialize();
            Console.WriteLine($"Deserialized item {JsonConvert.SerializeObject(deserializedContent)}");

            //saving custom contents and updating contents
            var newCustomContent = new Car();
            await Cms.ContentSystem.TrySaveAsync(newCustomContent);
            Console.WriteLine($"Saved custom content with id {newCustomContent.Id}");
            newCustomContent.Name = "meu custom name";
            await Cms.ContentSystem.TrySaveAsync(newCustomContent);
            Console.WriteLine($"Edited content: {JsonConvert.SerializeObject(Cms.ContentSystem.GetById(newCustomContent.Id))}");
        }

        static async Task FillDatabase(int toSave)
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
            await Cms.ContentSystem.TrySaveAsync(cacheList);
            Console.WriteLine($"{toSave} cars saved");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCTestApp.Models;
using CoreCMS;

namespace MVCTestApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("/inject")]
        public async Task<IActionResult> Inject()
        {
            //clean up database
            await Cms.ContentSystem.ClearAsync();
            
            //create candys
            for(int i = 0; i < 5; i++)
            {
                var newCandy = new Candy();
                await Cms.ContentSystem.TrySaveAsync(newCandy);

                var random = new Random();
                var chieldLollipops = random.Next(0, 60);
                for(int j =0; j < chieldLollipops; j++)
                {
                    var newLollipop = new Lollipop();
                    newLollipop.ParentId = newCandy.Id;
                    await Cms.ContentSystem.TrySaveAsync(newLollipop);
                }
            }

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

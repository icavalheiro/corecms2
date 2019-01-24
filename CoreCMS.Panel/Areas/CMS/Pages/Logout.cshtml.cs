using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCMS.Panel.CustomTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreCMS.Panel.Areas.CMS.Pages
{
    public class LogoutModel : PanelPageModel
    {
        public async Task<IActionResult> OnGet()
        {
            await LogoutUserAsync();
            return Redirect("/");
        }
    }
}

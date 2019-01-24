using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCMS.Panel.CustomTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreCMS.Panel.MyFeature.Pages
{
    public class IndexModel : PanelPageModel
    {
        public IActionResult OnGet()
        {
            if(GetUser() == null)
            {
                return Redirect("/cms/login");
            }

            return MyView;
        }
    }
}

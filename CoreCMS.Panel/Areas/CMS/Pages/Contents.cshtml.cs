using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCMS.Panel.CustomTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreCMS.Panel.Areas.CMS.Pages
{
    public class ContentsModel : PanelPageModel
    {
        public Content[] ContentsToShow;
        public int CurrentPage;
        public Content CurrentContent;

        public IActionResult OnGet([FromQuery] Guid id, [FromQuery] int page)
        {
            CurrentPage = page;
            if(id != Guid.Empty)
            {
                CurrentContent = Cms.ContentSystem.GetById(id);
                if(CurrentContent != null)
                {
                    ContentsToShow = Cms.ContentSystem.GetPage(CurrentContent, CurrentPage);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                //show root content if id was not set
                ContentsToShow = Cms.ContentSystem.GetPage(null, CurrentPage);
            }

            return MyView;
        }
    }
}

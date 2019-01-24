using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCMS.Panel.CustomTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreCMS.Panel.Areas.CMS.Pages
{
    public class LoginModel : PanelPageModel
    {
        public string LoginError;
        public string UsernameFromPost;
        public string PasswordFromPost;

        public IActionResult OnGet()
        {
            if (GetUser() != null)
                //user is already logged in... redirecting to home
                return Redirect("/cms");

            return MyView;
        }

        public async Task<IActionResult> OnPost([FromForm] string username, [FromForm] string password)
        {
            UsernameFromPost = username;
            PasswordFromPost = password;

            if(string.IsNullOrEmpty(username) 
                || string.IsNullOrEmpty(password) 
                || await TryLoginUserAsync(username, password) == false)
            {
                LoginError = "Please, enter a valid username and password";
                return MyView;
            }

            return Redirect("/cms/");
        }
    }
}

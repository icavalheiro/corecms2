using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreCMS.Panel.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCMS.Panel.CustomTypes
{
    /// <summary>
    /// Base class for the page models of the Razor Class Library in the CMS Panel.
    /// </summary>
    public class PanelPageModel : PageModel
    {
        private const string COOKIE_NAME = "CoreCMS_Login";

        public const IActionResult MyView = null;
        
        /// <summary>
        /// Get the current user for the current Request.
        /// </summary>
        /// <returns>The current user if it exists.</returns>
        public User GetUser()
        {
            return GetUserFromRequest();
        }

        /// <summary>
        /// Logout the current user from this session.
        /// </summary>
        public async Task LogoutUserAsync()
        {
            //get the current token
            var token = GetTokenFromRequest();
            if (token != null)
            {
                //delete it from our database
                await Cms.LoginTokenSystem.TryDeleteAsync(token);
            }

            //remove it from the client browser, so that it stop sending it to us
            Response.Cookies.Delete(COOKIE_NAME);
        }

        /// <summary>
        /// Logins the given user into the current session.
        /// </summary>
        /// <param name="user">User to get logged in.</param>
        public async Task LoginUserAsync(User user)
        {
            if(user == null || user.Id == Guid.Empty)
            {
                throw new Exception("Cannot login a user that is null or has not been saved in the database yet.");
            }

            //lets create a new token for this login
            var newToken = new LoginToken();
            newToken.UserId = user.Id;
            newToken.AccessIp = IpTools.TryGetRequestIP(HttpContext);

            //set it to expire in a week from now
            newToken.ExpireAt = DateTime.Now.AddDays(7);

            //lets save this token in the database
            //so that we can validade it later
            await Cms.LoginTokenSystem.TrySaveAsync(newToken);

            //Now, lets create the cookie that the user will hold to prove he is who he is
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = newToken.ExpireAt;

            //protection against javascript injections
            cookieOptions.HttpOnly = true;

            //protection against cross domain requests
            cookieOptions.SameSite = SameSiteMode.Strict;

            //force ssl only, to protect against middle man attacks
            cookieOptions.Secure = true;

            //append the cookie to the response
            Response.Cookies.Append(COOKIE_NAME, newToken.Id.ToString(), cookieOptions);
        }

        /// <summary>
        /// Tryies to login a user using an username and password.
        /// </summary>
        /// <param name="username">User's username.</param>
        /// <param name="password">User's password.</param>
        /// <returns>Wheter it was able to login the user or not.</returns>
        public async Task<bool> TryLoginUserAsync(string username, string password)
        {
            //lets retrieve the user from database
            var user = Cms.UserSystem.GetByUsername(username);
            if(user != null && user.TestPassword(password))
            {
                //valid username and valid password for this given user
                //lets login it then
                await LoginUserAsync(user);
                return true;
            }

            //not a valid username or password =S
            return false;
        }

        /// <summary>
        /// Gets the login token from current sessions using the Request.
        /// </summary>
        /// <returns>The token if it exists.</returns>
        private LoginToken GetTokenFromRequest()
        {
            //get the login cookie from the cookies we received from user's browser
            var cookie = Request.Cookies[COOKIE_NAME];

            //check if it valid
            if (!string.IsNullOrEmpty(cookie) && Guid.TryParse(cookie, out Guid result))
            {
                //retrieve the token instance from the database
                var tokenEntry = Cms.LoginTokenSystem.GetById(result);
                return tokenEntry;
            }

            //token not valid
            return null;
        }

        /// <summary>
        /// Gets the current logged user for this session using the Request.
        /// </summary>
        /// <returns>The user if it exists.</returns>
        private User GetUserFromRequest()
        {
            //we need the token that the user holds in his browser
            var tokenEntry = GetTokenFromRequest();
            if (tokenEntry != null)
            {
                //For security reasons we will check if the current request has the same IP
                //as who created the token (we shall only guarantee access to who created the token)
                //if this returns false, than we are prob. facing a cracker trying to use
                //someone else's credentials
                var requestIp = IpTools.TryGetRequestIP(HttpContext);

                //also checks if it is not expired yet
                if (requestIp == tokenEntry.AccessIp && tokenEntry.ExpireAt.Ticks > DateTime.Now.Ticks)
                {
                    //return the user related to that token
                    return Cms.UserSystem.GetById(tokenEntry.UserId);
                }
                else
                {
                    //if it has expired
                    //delete it from database since it is no longer valid
                    Task.Run(async () =>
                    {
                        await Cms.LoginTokenSystem.TryDeleteAsync(tokenEntry);
                    });
                }
            }

            //no token sent to us by the browser =S
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PicBook.Web.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace PicBook.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoginFacebook(string provider)
        {
            string[] providers = new string[] { "Facebook", "Twitter", "Microsoft" };
            if (!providers.Contains(provider))
                return Redirect(Url.Action("Login", "Account"));

            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("AuthCallback", "Account", new { provider = provider })
            };

            return Challenge(authenticationProperties, provider);
        }


        public IActionResult AuthCallback(string provider)
        {
            var facebookIdentity = User.Identities.FirstOrDefault(i => i.AuthenticationType == provider && i.IsAuthenticated);

            if (facebookIdentity == null)
            {
                return Redirect(Url.Action("Login", "Account"));
            }

            // facebookIdentity.Claims // TODO: <--- based on this, create a proprietary user account etc.
            
            return Redirect(Url.Action("Index", "Home"));
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect(Url.Action("Index", "Home"));
        }
    }
}
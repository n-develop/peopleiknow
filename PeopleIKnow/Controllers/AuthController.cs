using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PeopleIKnow.Models;

namespace PeopleIKnow.Controllers
{
    /*
     * Credits for most of the code in here goes to Mads Kristensen.
     * Most of the code is based on the AccountController of his MiniBlog.Core project.
     * https://github.com/madskristensen/Miniblog.Core/blob/master/src/Controllers/AccountController.cs
     * I like the code, because it is simple, secure and a perfect fit for a single user scenario like this.
     */
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/login")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [Route("/login")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(string returnUrl, LoginViewModel model)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid && model.UserName == _configuration["user:username"] &&
                VerifyHashedPassword(model.Password, _configuration))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, _configuration["user:username"]));

                var principle = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties {IsPersistent = model.RememberMe};
                await HttpContext.SignInAsync(principle, properties);

                return LocalRedirect(returnUrl ?? "/");
            }

            ModelState.AddModelError(string.Empty, "Username or password is invalid.");
            return View("login", model);
        }

        [Route("/logout")]
        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }

        [NonAction]
        private static bool VerifyHashedPassword(string password, IConfiguration config)
        {
            var saltBytes = Encoding.UTF8.GetBytes(config["user:salt"]);

            var hashBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
            );

            var hashText = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            return hashText == config["user:password"];
        }
    }
}
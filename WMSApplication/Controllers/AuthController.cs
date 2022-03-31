using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WMSApplication.Contracts;
using WMSApplication.CustomModels;
using WMSApplication.Models;

namespace WMSApplication.Controllers
{
    public class AuthController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        public AuthController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        public IActionResult Index(string returnUrl)
        {
            // prevent access login page once authenticated
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            AuthUser userAuth = new AuthUser();

            ViewData["ReturnUrl"] = returnUrl ?? Url.Action("Index", "Home");

            return View(userAuth);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthUser input, string returnUrl)
        {
            var userAuth = await _repository.AuthUser.FindAllAsync();

            bool isAuthenticate = CheckAuntentication(input, userAuth);

            if (!isAuthenticate)
            {
                TempData["failed"] = "Your credential doesn't authorized";
                return RedirectToAction(nameof(Index));
            }

            AuthModel authModel = CheckRoles(userAuth);

            if (authModel == null)
            {
                TempData["failed"] = "You don't have privillage to access resource. Check your role!";
                return RedirectToAction(nameof(Index));
            }

            SetCookies(authModel);

            // Redirect
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");

        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/");
        }

        [HttpGet("Denied")]
        public IActionResult Denied()
        {
            return View();
        }

        #region Support Method
        private async void SetCookies(AuthModel authModel)
        {
            // Create Claims 
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, authModel.Username));
            claims.Add(new Claim(ClaimTypes.Name, authModel.Name));
            claims.Add(new Claim(ClaimTypes.Email, authModel.Email));
            claims.Add(new Claim(ClaimTypes.Role, authModel.Role));

            // Create Identity
            var claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Create Principal 
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Sign In
            await HttpContext.SignInAsync(claimsPrincipal);
        }

        private AuthModel CheckRoles(IEnumerable<AuthUser> userAuth)
        {
            var roleAuth = _repository.AuthRole.FindAllAsync().Result;
            var userRoleAuth = _repository.AuthUserRole.FindAllAsync().Result;

            var query = from au in userAuth
                        join aur in userRoleAuth on au.Id equals aur.UserId into tableOne
                        from aur in tableOne.ToList()
                        join ar in roleAuth on aur.RoleId equals ar.Id into tableTwo
                        from ar in tableTwo.ToList()
                        select new AuthModel
                        {
                            Id = au.Id,
                            Name = au.Name,
                            Username = au.Username,
                            Email = au.Email,
                            Role = ar.Name
                        };

            AuthModel auth = query.FirstOrDefault();

            return auth;
        }

        private bool CheckAuntentication(AuthUser authUser, IEnumerable<AuthUser> userAuth)
        {
            return userAuth.ToList().Any(x => x.Username.Equals(authUser.Username) && x.Password.Equals(authUser.Password));
        }

        #endregion
    }
}

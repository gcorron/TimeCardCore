using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using TimeCard.Repo.Repos;
using TimeCardCore.Models;

namespace TimeCardCore.Controllers
{
    public class AccountController : BaseController
    {
        private readonly AppUserRepo _AppUserRepo;

        public AccountController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : base(config, webHostEnvironment, httpContextAccessor)
        {
            _AppUserRepo = new AppUserRepo(ConnString);
        }

        [HttpGet]
        [Authorize("Admin", "Read")]
        public IActionResult Index()
        {
            var vm = new AppUserViewModel { EditAppUser = new TimeCard.Domain.AppUser() };
            PrepAppUser(vm);
            return View(vm);
        }

        private void PrepAppUser(AppUserViewModel vm)
        {
            vm.AppUsers = _AppUserRepo.GetAppUsers().OrderBy(x => x.UserName);
        }

        public IActionResult AccessDenied()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            var login = _AppUserRepo.Login(vm.UserName, vm.Password);
            string message="Internal error";
            switch (login.Result)
            {
                case "OK":
                    var roles = _AppUserRepo.GetRoles(login.UserId);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, vm.UserName),
                        new Claim("UserId",login.UserId.ToString()),
                        new Claim("FullName",login.UserFullName),
                        new Claim("ContractorId",login.ContractorId.ToString())
                    };
                    foreach(var role in roles)
                    {
                        claims.Add(new Claim("Role", role));
                    }
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = vm.RememberMe,
                        IssuedUtc = login.LoginTime,
                        RedirectUri = "/",
                        ExpiresUtc = DateTimeOffset.Now.AddDays(99999)
                    };
                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    return Redirect("/");
                case "NO":
                    message = "User Name or Password is invalid.";
                    break;
                case "LOCKOUT":
                    message = "Maximum tries exceeded.";
                    break;
            }
            ModelState.AddModelError("failed", message);
            return View("AccessDenied", vm);
        }

    }
}
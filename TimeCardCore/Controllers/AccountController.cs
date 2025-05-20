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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using TimeCard.Domain;
using TimeCard.Repo.Repos;
using TimeCardCore.Infrastructure;
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
            var vm = new AppUserViewModel();
            PrepAppUser(vm,false);
            return View(vm);
        }

        [HttpPost]
        [Authorize("Admin", "Write")]
        public IActionResult Index(AppUserViewModel vm, string buttonValue)
        {
            bool retainEdit = false;
            switch (buttonValue)
            {
                case "Save":
                    if (ModelState.IsValid)
                    {
                        int userId =_AppUserRepo.SaveAppUser(vm.EditAppUser);
                        _AppUserRepo.DeleteUserRoles(userId);
                        foreach(var role in vm.EditAppUser.Roles.Where(x => x.Active))
                        {
                            _AppUserRepo.SaveUserRole(userId, role.Id);
                        }
                    }
                    else
                    {
                        retainEdit = true;
                    }
                    break;
                case "Delete":
                    _AppUserRepo.DeleteAppUser(vm.EditAppUser.UserId);
                    break;
            }
            PrepAppUser(vm,retainEdit);
            return View(vm);
        }




        private void PrepAppUser(AppUserViewModel vm, bool retainEdit)
        {
            
            vm.AppUsers = _AppUserRepo.GetAppUsers().OrderBy(x => x.UserName);
            foreach(var user in vm.AppUsers)
            {
                user.Roles = _AppUserRepo.GetUserRoles(user.UserId).Where(x => x.Active).OrderBy(x => x.Descr).ToArray();
            }

            if (!retainEdit)
            {
                vm.EditAppUser = new TimeCard.Domain.AppUser { Active = true };
                ModelState.Clear();
            }
            vm.EditAppUser.Roles = _AppUserRepo.GetUserRoles(vm.EditAppUser.UserId).ToArray();
        }

        public IActionResult AccessDenied()
        {
            return Redirect("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _httpContext.Session.Clear();
            CurrentIdentity = null;
            return Ok();
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            string message="Internal error";
            if (vm.Reset && (vm.NewPassword == null || vm.NewPassword != vm.ConfirmNewPassword))
            {
                message = vm.NewPassword == null ? "New Password Required." : "Passwords do not match";
            }
            else
            {
                var login = _AppUserRepo.Login(vm.UserName, vm.Password, vm.Reset ? vm.NewPassword : null);
                switch (login.Result)
                {
                    case "RESET":
                        vm.Reset = true;
                        ModelState.Clear();
                        message = "Please change your password.";
                        break;
                    case "OK":
                        var identity = new Identity
                        {
                            Roles = _AppUserRepo.GetUserRoles(login.UserId).Where(x => x.Active).Select(x => x.Descr),
                            UserId = login.UserId,
                            UserFullName = login.UserFullName,
                            UserName = vm.UserName,
                            ContractorId = login.ContractorId,
                            UserContractorId = login.ContractorId,
                            Token = NewToken(login.UserId, vm.UserName, 10 * 60 * 60)
                        };
                        CurrentIdentity = identity;
                        return Redirect("/Work");
                    case "NO":
                        message = "User Name or Password is invalid.";
                        break;
                    case "LOCKOUT":
                        message = "Maximum tries exceeded.";
                        break;
                }
            }

            ModelState.AddModelError("failed", message);
            return View(vm);
        }
        [Authorize("Admin","Read")]
        public async void ChangeWorker(int contractorId, string contractorName)
        {
            var identity = CurrentIdentity;
            if (identity == null)
            {
                return;
            }
            identity.ContractorId = contractorId;
            identity.ContractorName = contractorName;
            CurrentIdentity = identity;
        }

        [HttpPost]
        public IActionResult GetContractor(int userId)
        {
            var contractor = _AppUserRepo.GetContractor(userId);
            if (contractor == null)
            {
                contractor = new Contractor { ContractorId = userId };
            }
            return PartialView("_ContractorModal", contractor);
        }

        [HttpPost]
        public IActionResult SaveContractor(Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                _AppUserRepo.SaveContractor(contractor);
            }
            return PartialView("_ContractorModal",contractor);
        }
        public ActionResult SelectWorker()
        {
            var vm = _AppUserRepo.GetUsers().Select(x => new SelectListItem { Text = x.UserName, Value = x.ContractorId.ToString(), Selected = x.ContractorId == CurrentIdentity.ContractorId });
            return PartialView("_WorkerModal", vm);
        }

    }
}
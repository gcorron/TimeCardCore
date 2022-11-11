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
using Microsoft.AspNetCore.Mvc.Rendering;
using TimeCard.Domain;
using TimeCard.Repo.Repos;
using TimeCardCore.Infrastructure;
using TimeCardCore.Models;

namespace TimeCardCore.Controllers
{
    public class BudgetController : BaseController
    {
        private readonly BudgetRepo _BudgetRepo;
        private readonly JobRepo _JobRepo;
        private readonly LookupRepo _LookupRepo;
        public BudgetController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : base(config, webHostEnvironment, httpContextAccessor)
        {
            _BudgetRepo = new BudgetRepo(ConnString);
            _JobRepo = new JobRepo(ConnString);
            _LookupRepo = new LookupRepo(ConnString);
        }
        public IActionResult Index()
        {
            var vm = new BudgetViewModel
            {
                Active = true
            };
            prepBudget(vm);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Index(BudgetViewModel vm, string buttonValue="")
        {
            switch(buttonValue)
            {
                case "Save":
                    _BudgetRepo.UpdateBudget(vm.EditBudget);
                    break;
                case "Delete":
                    break;
                default:
                    break;
            }
            prepBudget(vm);
            return View(vm);
        }

        private void prepBudget(BudgetViewModel vm)
        {
            vm.Budgets = _BudgetRepo.GetBudgets(vm.Active);
            if (vm.Action == "Edit")
            {
                vm.EditBudget = vm.Budgets.FirstOrDefault(x => x.BudgetId == vm.ActionId);
            }
            else
            {
                vm.EditBudget = new Budget { Active = true };
            }
            vm.Jobs = Enumerable.Repeat(new SelectListItem { Text="- Select -", Value="0"},1).Union(_JobRepo.GetJobStart(0).OrderBy(x => x.Descr).Select(x => new SelectListItem { Text = x.Descr, Value = x.JobId.ToString() }));
            vm.BudgetTypes = _LookupRepo.GetLookups("Budget","- Select -").OrderBy(x => x.Val).Select(x => new SelectListItem { Text = x.Descr, Value = x.Id.ToString() });
        }
    }
}

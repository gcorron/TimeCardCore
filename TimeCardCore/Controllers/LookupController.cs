using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TimeCard.Repo.Repos;
using TimeCardCore.Infrastructure;
using TimeCardCore.Models;

namespace TimeCardCore.Controllers
{
    [Authorize("Admin","Read")]
    public class LookupController : BaseController
    {
        public LookupController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : base(config, webHostEnvironment, httpContextAccessor)
        {

        }
        [HttpGet]
        public IActionResult Index()
        {
            var vm = new LookupViewModel { EditLookup = new TimeCard.Domain.Lookup() };
            prepIndex(vm);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Index(LookupViewModel vm, string buttonValue)
        {
            switch (buttonValue)
            {
                case "Save":
                    if (ModelState.IsValid)
                    {
                        vm.EditLookup.GroupId = vm.SelectedGroupId;
                        LookupRepo.SaveLookup(vm.EditLookup);
                        vm.EditLookup = new TimeCard.Domain.Lookup();
                        ModelState.Clear();
                    }
                    break;
                case "Delete":
                    LookupRepo.DeleteLookup(vm.EditLookup.Id);
                    break;
                default:
                    vm.EditLookup = new TimeCard.Domain.Lookup();
                    ModelState.Clear();
                    break;
            }
            prepIndex(vm);
            return View(vm);
        }
        void prepIndex(LookupViewModel vm)
        {
            vm.LookupGroups = LookupRepo.GetGroups().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = x.GroupId.ToString(), Text = x.Descr });
            if (vm.SelectedGroupId !=0)
            {
                vm.Lookups = LookupRepo.GetLookups(vm.SelectedGroupId);
            }
        }
    }
}
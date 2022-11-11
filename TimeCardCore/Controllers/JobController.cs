using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using TimeCard.Repo.Repos;
using TimeCardCore.Infrastructure;
using TimeCardCore.Models;

namespace TimeCardCore.Controllers
{
    [Authorize("Contractor", "Read")]
    public class JobController : BaseController
    {
        private readonly JobRepo _JobRepo;

        public JobController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : base(config, webHostEnvironment, httpContextAccessor)
        {
            _JobRepo = new JobRepo(ConnString);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var vm = new Models.JobViewModel { ContractorId = ContractorId, Active = true};
            return Index(vm);
        }

        [HttpPost]
        public ActionResult Index(JobViewModel vm)
        {
            vm.Jobs = _JobRepo.GetJobStart(ContractorId, vm.Active);
            return View(vm);

        }

        [HttpPost]
        public void SetJobDate(int contractorId, int jobId, string theDate, bool isNew)
        {
            decimal startDay = 0;
            if (!String.IsNullOrEmpty(theDate))
            {
                DateTime BaselineDate = new DateTime(2018, 12, 22);
                DateTime startDate = DateTime.Parse(theDate);
                int days = (startDate - BaselineDate).Days;
                startDay = days / 14 + (days % 14) * (decimal)0.01;
            }
            _JobRepo.UpdateJobStart(contractorId, jobId, startDay, isNew);
        }

        [HttpPost]
        public void SaveDescr(int contractorId, int jobId, string descr)
        {
            _JobRepo.SaveJobDescr(contractorId, jobId, descr);
        }

        [HttpPost]
        [Authorize("Admin", "Write")]
        public ActionResult AddJob()
        {
            var vm = new JobAddViewModel();
            PrepAddJob(vm);
            return PartialView("_JobModal", vm);
        }

        [HttpPost]
        [Authorize("Admin", "Write")]
        public ActionResult SaveJob(JobAddViewModel vm)
        {
            ModelState.Clear();
            if (vm.SelectedClientId == 0)
            {
                ModelState.AddModelError("SelectedClientId", "Please select a client.");
            }
            if (vm.SelectedProjectId == 0)
            {
                ModelState.AddModelError("SelectedProjectId", "Please select a project.");
            }
            if (vm.SelectedBillTypeId == 0)
            {
                ModelState.AddModelError("SelectedBillTypeId", "Please select a bill type.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _JobRepo.SaveJob(vm.SelectedClientId, vm.SelectedProjectId, vm.SelectedBillTypeId);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("All", ex.Message);
                }
            }

            PrepAddJob(vm);
            return PartialView("_JobModal", vm);
        }

        public ActionResult DeleteJob(int jobId)
        {
            try
            {
                _JobRepo.DeleteJob(jobId);
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private void PrepAddJob(JobAddViewModel vm)
        {
            vm.Clients = GetLookup("Client");
            vm.Projects = GetLookup("Project");
            vm.BillTypes = GetLookup("BillType");
            vm.Jobs = (new TimeCard.Domain.Lookup[] { new TimeCard.Domain.Lookup { Id = 0, Descr = "- Select -" } }.Union(_JobRepo.GetJobsUnused())
                .Select(x => new SelectListItem { Text = x.Descr, Value = x.Id.ToString() }));
        }


        private IEnumerable<SelectListItem> GetLookup(string group)
        {
            return LookupRepo.GetLookups(group, "- Select -").Where(x => x.Id == 0 || x.Active == true)
                .Select(x => new SelectListItem { Text = x.Descr, Value = x.Id.ToString() });
        }
    }
}




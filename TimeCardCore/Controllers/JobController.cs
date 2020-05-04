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

        public ActionResult Index()
        {
            var jobs = _JobRepo.GetJobStart(ContractorId);
            return View(new Models.JobViewModel { ContractorId = ContractorId, Jobs = jobs });
        }

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
    }

}


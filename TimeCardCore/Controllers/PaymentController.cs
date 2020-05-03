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
using TimeCardCore.Models;

namespace TimeCardCore.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly PaymentRepo _PaymentRepo;
        private readonly WorkRepo _WorkRepo;
        private readonly JobRepo _JobRepo;
        private readonly LookupRepo _LookupRepo;

        public PaymentController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : base(config, webHostEnvironment, httpContextAccessor)
        {
            _PaymentRepo = new PaymentRepo(ConnString);
            _WorkRepo = new WorkRepo(ConnString);
            _JobRepo = new JobRepo(ConnString);
            _LookupRepo = new LookupRepo(ConnString);
        }

        [HttpGet]
        public ActionResult Index()
        {

            var vm = new PaymentViewModel
            {
                SelectedContractorId = ContractorId,
                IsAdmin = false,
                EditPayment = new TimeCard.Domain.Payment { ContractorId = ContractorId },
                PaymentSummary = _PaymentRepo.GetSummary(ContractorId),
                Payments = _PaymentRepo.GetPayments(ContractorId)
            };
            prepPayment(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(PaymentViewModel vm, string buttonValue)
        {
            switch (buttonValue)
            {
                case "Save":

                    if (vm.JobIsTimeCard)
                    {
                        if (vm.EditPayment.WorkDay == 0)
                        {
                            ModelState.AddModelError("EditPayment.WorkDay", "Please select Work Period");
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        vm.EditPayment.ContractorId = vm.SelectedContractorId;
                        vm.EditPayment.JobId = vm.SelectedJobId;
                        _PaymentRepo.SavePayment(vm.EditPayment);
                        vm.EditPayment = new TimeCard.Domain.Payment { PayDate = vm.EditPayment.PayDate };
                        ModelState.Clear();
                    }
                    break;
                case "Delete":
                    _PaymentRepo.DeletePayment(vm.EditPayment.PayId);
                    vm.EditPayment = new TimeCard.Domain.Payment { PayDate = vm.EditPayment.PayDate };
                    ModelState.Clear();
                    break;
                case "Summary":
                    vm.PaymentSummary = _PaymentRepo.GetSummary(vm.SelectedContractorId);
                    vm.Payments = _PaymentRepo.GetPayments(vm.SelectedContractorId);
                    prepPayment(vm);
                    return PartialView("_PaymentSummary", vm);
                default:
                    vm.EditPayment = new TimeCard.Domain.Payment { PayDate = vm.EditPayment.PayDate };
                    ModelState.Clear();
                    break;
            }
            prepPayment(vm);
            return PartialView("_EditPayment", vm);
        }

        private void prepPayment(PaymentViewModel vm)
        {
            vm.JobIsTimeCard = false;
            vm.TimeCardsUnpaid = null;
            if (vm.SelectedJobId != 0 && vm.SelectedContractorId != 0)
            {
                var job = _JobRepo.GetJob(vm.SelectedJobId);
                vm.SelectedJob = job;
                vm.JobIsTimeCard = (job.BillTypeDescr == "TC");
                if (vm.JobIsTimeCard)
                {
                    vm.TimeCardsUnpaid = _PaymentRepo.GetJobTimeCardUnpaidCycles(vm.SelectedContractorId, vm.SelectedJobId).Select(x => new SelectListItem { Text = x.ToString(), Value = x.WorkDay.ToString() });
                }
            }
            if (vm.Payments == null)
            {
                if (vm.SelectedJobId == 0 || vm.SelectedContractorId == 0)
                {
                    vm.Payments = Enumerable.Empty<TimeCard.Domain.Payment>();
                }
                else
                {
                    vm.Payments = _PaymentRepo.GetPaymentsForJob(vm.SelectedContractorId, vm.SelectedJobId);
                }
            }
        }
    }
}
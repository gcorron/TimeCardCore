using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TimeCard.Helpers;
using TimeCard.Repo.Repos;
using OfficeOpenXml;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using TimeCardCore.Infrastructure;
using System.Text.RegularExpressions;
using OfficeOpenXml.Core;

namespace TimeCardCore.Controllers;

[Authorize("Contractor", "Read")]
public class WorkController : BaseController
{
    private readonly WorkRepo _WorkRepo;
    private readonly PaymentRepo _PaymentRepo;
    private readonly JobRepo _JobRepo;
    private readonly AppUserRepo _AppUserRepo;
    private readonly BudgetRepo _BudgetRepo;

    public WorkController(IConfiguration config, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : base(config, webHostEnvironment, httpContextAccessor)
    {
        _WorkRepo = new WorkRepo(ConnString);
        _PaymentRepo = new PaymentRepo(ConnString);
        _JobRepo = new JobRepo(ConnString);
        _AppUserRepo = new AppUserRepo(ConnString);
        _BudgetRepo = new BudgetRepo(ConnString);
    }

    public IActionResult Index()
    {
        var vm = new Models.WorkViewModel { SelectedContractorId = CurrentIdentity.ContractorId, SelectedContractorDescr = CurrentIdentity.UserName, EditPermission = CurrentIdentity.ContractorId == CurrentIdentity.UserContractorId ? "Full" : "Admin" };
        prepWork(vm);
        return View(vm);
    }

    [HttpPost]
    public ActionResult Index(Models.WorkViewModel vm, string buttonValue)
    {
        bool clearEdit = false;
        switch (buttonValue)
        {
            case "OpenClose":
                _WorkRepo.ToggleWorkOpen(vm.SelectedContractorId, vm.SelectedCycle);
                ModelState.Clear();
                break;
            case "Save":
                if (0 == (vm.WorkTypeBudget ? vm.EditWork.BudgetId : vm.EditWork.JobId))
                {
                    ModelState.AddModelError("EditWork.BudgetId", "Please select a Job.");
                }
                if (ModelState.IsValid)
                {
                    var work = vm.EditWork;
                    clearEdit = work.WorkId == 0;
                    _WorkRepo.SaveWork(work);
                    if (!clearEdit)
                    {
                        vm.EditWork = null;
                    }
                    ModelState.Clear();
                }
                break;
            case "Delete":
                _WorkRepo.DeleteWork(vm.EditWork.WorkId);
                vm.EditWork = null;
                ModelState.Clear();
                break;
            case "Refresh":
                ModelState.Clear();
                break;
            default:
                vm.EditWork = null;
                ModelState.Clear();
                break;
        }
        prepWork(vm, clearEdit);
        return View(vm);
    }

    private void prepWork(Models.WorkViewModel vm, bool clearEdit = false)
    {
        var cycles = GetPayCycles();
        int cycle = int.Parse(cycles.First().Value);
        var workTypes = LookupRepo.GetLookups("WorkType", "- Select -");
        vm.WorkTypes = workTypes.Select(x => new SelectListItem { Text = x.Descr, Value = x.Id.ToString() });
        vm.PayCycles = cycles;
        vm.IsCycleOpen = false;
        vm.CanCloseCycle = true;
        if (vm.SelectedCycle == 0)
        {
            vm.SelectedCycle = cycle;
        }
        if (vm.SelectedCycle == cycle)
        {
            vm.IsCycleOpen = true;
            vm.CanCloseCycle = false;
        }

        vm.WorkEntries = _WorkRepo.GetWork(vm.SelectedContractorId, vm.SelectedCycle, true);
        if (vm.SortByJob)
        {
            vm.WorkEntries = vm.WorkEntries.OrderBy(x => x.Job).ThenBy(x => x.WorkDay);
        }
        if (vm.EditWork == null)
        {
            vm.EditWork = new TimeCard.Domain.Work { ContractorId = vm.SelectedContractorId, WorkDay = DateRef.GetWorkDay(DateTime.Today) };
        }
        if (clearEdit)
        {
            vm.EditWork = new TimeCard.Domain.Work { ContractorId = vm.SelectedContractorId, WorkDay = vm.EditWork.WorkDay, JobId = vm.EditWork.JobId, WorkType = vm.EditWork.WorkType };
        }
        vm.EditDays = GetEditDays(vm.SelectedCycle);
        vm.DailyTotals = new decimal[2][];
        for (int i = 0; i < 2; i++)
        {
            vm.DailyTotals[i] = new decimal[8];
            for (int j = 0; j < 7; j++)
            {
                vm.DailyTotals[i][j] = vm.WorkEntries.Where(x => x.WeekDay == j + i * 7).Sum(x => x.Hours);
                vm.DailyTotals[i][7] += vm.DailyTotals[i][j];
            }
        }

        if (!vm.IsCycleOpen)
        {
            vm.IsCycleOpen = _WorkRepo.GetWorkOpen(vm.SelectedContractorId).Any(x => x == vm.SelectedCycle);
        }

        vm.WorkTypeBudget = workTypes.FirstOrDefault(x => x.Val == "BUDG").Id == vm.EditWork.WorkType;
        if (vm.WorkTypeBudget)
        {
            vm.Budgets = Enumerable.Repeat(new SelectListItem { Text = "- Select -", Value = "0" }, 1).Union(_BudgetRepo.GetBudgets(true, CurrentIdentity.ContractorId).Select(x => new SelectListItem { Text = x.Descr, Value = x.BudgetId.ToString() }));
        }
        else
        {
            vm.Jobs = _JobRepo.GetJobsForWork(vm.SelectedContractorId, vm.SelectedCycle).Select(x => new SelectListItem { Text = x.Descr, Value = x.Id.ToString() });
        }

    }

    private IEnumerable<SelectListItem> GetEditDays(int thisCycle)
    {
        return Enumerable.Range(0, 14).Select(x => new SelectListItem { Text = $"{DateRef.GetWorkDate(thisCycle + (decimal)x / 100):MM/dd}", Value = (thisCycle + (decimal)x / 100).ToString() });
    }

    private IEnumerable<SelectListItem> GetPayCycles()
    {
        var thisCycle = decimal.Floor(DateRef.GetWorkDay(DateTime.Today));
        return Enumerable.Range(0, 15).Select(x => new SelectListItem { Text = $"{DateRef.PeriodEndDate(thisCycle - x):MM/dd/yyyy}", Value = (thisCycle - x).ToString() });
    }

    [HttpPost]
    public ActionResult WorkSummary(int contractorId)
    {
        var summary = _WorkRepo.GetWorkSummary(contractorId);
        return PartialView("_WorkSummaryPeriod", summary);
    }

    [HttpPost]
    public ActionResult WorkSummaryJob(int contractorId)
    {
        var summary = _WorkRepo.GetWorkSummary(contractorId);
        return PartialView("_WorkSummaryJob", summary);
    }

    [HttpPost]
    public ActionResult WorkDetailJob(int contractorId, int jobId)
    {
        var detail = _WorkRepo.GetWorkJobDetail(contractorId, jobId);
        return PartialView("_WorkDetailJob", detail);
    }
    [HttpPost]
    public ActionResult GenerateDocs(int contractorId, int cycle, int weeks)
    {
        var templateFile = new FileInfo($"{WebRootPath}\\Content\\TimeCardTemplates.xlsx");
        try
        {
            string name = LookupRepo.GetLookups("Contractor").Where(x => x.Id == contractorId).FirstOrDefault()?.Descr;

            var fileList = new List<string>();
            GenerateTimeCards(contractorId, name, cycle, fileList, weeks);
            GenerateTimeBooks(contractorId, name, templateFile, cycle, fileList);
            if (!fileList.Any())
            {
                return Json(new { success = false, message = "Nothing to generate." });
            }


            string zipFile = $"{name}.zip";
            return Json(new { success = true, obj = new ZipDownload { FileName = zipFile, FileList = fileList } });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }

    }

    private string GetTimeCardTabName(int week)
    {
        return $"C1 W{week + 1}";
    }
    private void GenerateTimeCards(int contractorId, string name, int cycle, List<string> fileList, int weeks)
    {
        var templateFile = new FileInfo($"{WebRootPath}\\Content\\BigRockTemplate.xlsx");
        DateTime startDate = DateRef.GetWorkDate(cycle);
        DateTime endDate = startDate.AddDays(weeks * 7 - 1);
        var entries = _WorkRepo.GetWorkByDate(contractorId, startDate, endDate);
        var workEntries = entries
            .Where(x => x.BillType == "TC")
            .GroupBy(g => new { g.ClientId, g.ProjectId });

        using (var xlTemplate = new ExcelPackage(templateFile))
        {
            foreach (var tc in workEntries) // client and project group
            {
                int blankRow = 14;
                //create a new time card and populate it
                var file = new FileInfo($"{DocsFolder()}\\bigrock_tc_gc_{startDate:MMddyy}_{endDate:MMddyy}.xlsx");
                System.IO.File.Delete(file.FullName);
                ExcelWorksheet sheet = null;
                using (var package = new ExcelPackage(file, templateFile))
                {
                    var workBook = package.Workbook;
                    var first = tc.First();
                    ExcelWorksheet templateSheet;
                    for (int i = 0; i < weeks; i++)
                    {
                        int currentRow = blankRow + 1;
                        string tabName = GetTimeCardTabName(i);
                        sheet = package.Workbook.Worksheets[i];
                        sheet.Cells[8, 11].Value = startDate.AddDays(i * 7 + 6);
                        sheet.Cells[8, 1].Value = first.Contractor;
                        sheet.Cells[8, 7].Value = first.Client;
                        sheet.Cells[8, 9].Value = "Richard Bravo";
                        sheet.Cells[19, 12].Value = DateTime.Today;
                        sheet.Cells[19, 2].Value = first.Contractor;
                        for (int j = 0; j < 7; j++)
                        {
                            sheet.Cells[10, 7 + j].Value = startDate.AddDays(i * 7 + j).ToString("ddd");
                            sheet.Cells[11, 7 + j].Value = startDate.AddDays(i * 7 + j).ToString("MM/dd");
                        }
                        foreach (var entry in tc.Where(x => x.WorkDate >= startDate.AddDays(i * 7) && x.WorkDate <= startDate.AddDays(6 + i * 7)).Reverse())
                        {
                            var wd = (entry.WorkDate - startDate.AddDays(i * 7)).Days;
                            sheet.InsertRow(currentRow, 1);
                            sheet.Cells[blankRow, 1, blankRow, 20].Copy(sheet.Cells[currentRow, 1]);
                            sheet.Cells[currentRow, 2].Value = entry.Descr;
                            sheet.Cells[currentRow, 4].Value = entry.Project;
                            sheet.Cells[currentRow, 7 + wd].Value = entry.Hours;
                            sheet.Cells[currentRow, 7 + wd].Value = entry.Hours;
                        }
                        sheet.DeleteRow(blankRow);
                    }
                    sheet.Calculate();
                    if (package.Workbook.Worksheets.Any())
                    {
                        package.Workbook.Calculate();
                        package.Save();
                        fileList.Add(package.File.FullName);
                    }
                }
            }
        }
    }

    public void GenerateTimeBooks(int contractorId, string name, FileInfo templateFile, int cycle, List<string> fileList)
    {
        var workEntries = _WorkRepo.GetWorkExtended(contractorId, cycle, false).Where(x => "SOW TB".Contains(x.BillType))
            .GroupBy(g => new { g.JobId, g.ClientId, g.ProjectId })
            .OrderBy(x => x.Key.ClientId);
        using (var templatePackage = new ExcelPackage(templateFile))
        {
            var templateSheet = templatePackage.Workbook.Worksheets["TimeBook"];
            var file = new FileInfo($"{DocsFolder()}\\{name}_TimeBooks.xlsx");
            System.IO.File.Delete(file.FullName);

            using (var package = new ExcelPackage(file))
            {
                foreach (var tb in workEntries)
                {
                    var first = tb.First();
                    var workBook = package.Workbook;
                    int currentRow = 2;
                    ExcelWorksheet sheet = workBook.Worksheets.Add($"{first.Job}", templateSheet);
                    //ExcelWorksheet sheet = workBook.Worksheets.Add($"{first.Client} {(first.Client == "Sessions" && (first.Project.StartsWith("Open") || first.Project.StartsWith("Extra")) ? "" : first.Project)}", templateSheet);
                    foreach (var entry in tb)
                    {
                        sheet.Cells[currentRow, 1].Value = $"{entry.WorkDate:M/d/yy}";
                        sheet.Cells[currentRow, 2].Value = entry.Hours;
                        sheet.Cells[currentRow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        sheet.Cells[currentRow, 3].Value = entry.WorkType;
                        sheet.Cells[currentRow, 4].Value = entry.BudgetType ?? entry.Project;
                        sheet.Cells[currentRow, 4].Style.WrapText = true;
                        sheet.Cells[currentRow, 5].Value = entry.Descr;
                        sheet.Cells[currentRow, 5].Style.WrapText = true;
                        sheet.Row(currentRow).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                        currentRow++;
                    }
                }
                if (package.Workbook.Worksheets.Any())
                {
                    package.Save();
                    fileList.Add(package.File.FullName);
                }
            }
        }
    }

    public void GenerateSummary(int contractorId, string name, FileInfo templateFile, int cycle, List<string> fileList)
    {
        var workSummary = _WorkRepo.GetWorkSummary(contractorId, true).Where(x => x.WorkPeriod < DateRef.CurrentWorkCycle);
        var file = new FileInfo($"{DocsFolder()} \\{name}_Summary.xlsx");
        System.IO.File.Delete(file.FullName);
        using (var templatePackage = new ExcelPackage(templateFile))
        {
            var templateSheet = templatePackage.Workbook.Worksheets["Summary"];

            using (var package = new ExcelPackage(file))
            {
                var workBook = package.Workbook;
                ExcelWorksheet sheet = workBook.Worksheets.Add("Summary", templateSheet);
                int currentRow = 2;
                foreach (var group in workSummary.OrderBy(x => x.Descr).ThenBy(x => x.WorkPeriod).GroupBy(x => x.JobId))
                {
                    int i = 0;
                    decimal total = 0;
                    foreach (var row in group)
                    {
                        total += row.Hours;
                        sheet.InsertRow(currentRow, 1, currentRow);
                        sheet.Cells[currentRow, 2].Value = row.WorkPeriodDescr;
                        sheet.Cells[currentRow, 3].Value = row.Hours;
                        sheet.Cells[currentRow, 4].Value = total;
                        sheet.Cells[currentRow, 3].Style.Numberformat.Format = "0.00";
                        sheet.Cells[currentRow, 4].Style.Numberformat.Format = "0.00";
                        if (i == group.Count() - 1)
                        {
                            sheet.Cells[currentRow, 4].Style.Font.Bold = true;
                            sheet.Cells[$"A{currentRow - group.Count() + 1}:A{currentRow}"].Merge = true;
                            sheet.Cells[currentRow - group.Count() + 1, 1].Value = row.Descr;
                        }
                        i++;
                        currentRow++;
                    }
                }
                currentRow += 5;

                var paySummary = _PaymentRepo.GetSummary(contractorId, DateRef.CurrentWorkCycle)
                  .OrderBy(x => x.Client).ThenBy(x => x.Project).ThenBy(x => x.BillType);
                var payments = _PaymentRepo.GetPayments(contractorId);
                foreach (var summary in paySummary)
                {
                    sheet.InsertRow(currentRow, 1, currentRow);
                    sheet.Cells[currentRow, 1].Value = summary.Client;
                    sheet.Cells[currentRow, 2].Value = summary.Project;
                    sheet.Cells[currentRow, 3].Value = summary.BillType;
                    sheet.Cells[currentRow, 4].Value = summary.Billed;
                    sheet.Cells[currentRow, 5].Value = summary.Paid;
                    sheet.Cells[currentRow, 6].Value = summary.Balance;
                    sheet.Cells[currentRow, 7].Value = summary.StartDate;
                    sheet.Cells[currentRow, 8].Value = summary.PaidThruDate;
                    var jobPayments = payments.Where(x => x.JobId == summary.JobId)
                        .OrderBy(x => x.PayDate);
                    if (jobPayments.Any())
                    {
                        foreach (var jp in jobPayments)
                        {
                            sheet.Cells[currentRow, 9].Value = jp.Hours;
                            sheet.Cells[currentRow, 10].Value = $"{jp.PayDate:MM/dd/yyyy}";
                            sheet.Cells[currentRow, 11].Value = jp.CheckNo;
                            currentRow++;
                            sheet.InsertRow(currentRow, 1, currentRow);
                        }
                        if (jobPayments.Count() > 1)
                        {
                            for (int i = 0; i <= 7; i++)
                            {
                                char col = "ABCDEFGH"[i];
                                sheet.Cells[$"{col}{currentRow - jobPayments.Count()}:{col}{currentRow - 1}"].Merge = true;
                            }
                            sheet.Cells[$"A{currentRow - jobPayments.Count()}:H{currentRow - 1}"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                        }
                    }
                    else
                    {
                        currentRow++;
                    }
                }
                if (package.Workbook.Worksheets.Any())
                {
                    package.Save();
                    fileList.Add(package.File.FullName);
                }
            }
        }
    }

    public async Task<ActionResult> DownloadTimeDocs([FromBody] ZipDownload download)
    {

        var buffer = new byte[4096];
        byte[] retBytes = null;
        using (var stream = new MemoryStream())
        {
            using (var zipOutputStream = new ZipOutputStream(stream))
            {
                // 0-9, 9 being the highest level of compression
                zipOutputStream.SetLevel(3);
                foreach (string fileName in download.FileList)
                {

                    using (Stream fs = System.IO.File.OpenRead(fileName))
                    {

                        var entry = new ZipEntry(ZipEntry.CleanName(Path.GetFileName(fileName)));
                        entry.Size = fs.Length;
                        // Setting the Size provides WinXP built-in extractor 
                        // compatibility, but if not available, you can instead set 
                        //zipOutputStream.UseZip64 = UseZip64.Off

                        await zipOutputStream.PutNextEntryAsync(entry);

                        int count = await fs.ReadAsync(buffer, 0, buffer.Length);
                        while (count > 0)
                        {
                            await zipOutputStream.WriteAsync(buffer, 0, count);
                            count = await fs.ReadAsync(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
            retBytes = stream.ToArray();
        }
        return File(retBytes, "application/zip", download.FileName);
    }
    private string DocsFolder()
    {
        return $"{WebRootPath}\\Docs";
        ;
    }
}
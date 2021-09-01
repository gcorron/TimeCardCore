using System;
using System.ComponentModel.DataAnnotations;
using TimeCard.Helpers;
namespace TimeCard.Domain
{
    public class WorkExtended
    {
        public int WorkId { get; set; }
        public int ContractorId { get; set; }
        public int JobId { get; set; }
        public decimal WorkDay { get; set; }
        public string Descr { get; set; }
        public decimal Hours { get; set; }
        public int Cycle { get => (int)Decimal.Floor(WorkDay); }
        public string CycleEndDate { get => $"{DateRef.PeriodEndDate(WorkDay):MM/dd/yy}"; }
        public string TimeCardDateString { get => $"{DateRef.PeriodEndDate(WorkDay).AddDays(-7):MMdd}_{DateRef.PeriodEndDate(WorkDay):MMdd}"; }
        public string TimeCardDateString4 { get => $"{DateRef.PeriodEndDate(WorkDay).AddDays(-21):MMdd}_{DateRef.PeriodEndDate(WorkDay):MMdd}"; }
        public DateTime WorkDate { get => DateRef.GetWorkDate((decimal)WorkDay); }
        public int WorkWeek { get => (WorkDay % 1) < (decimal)0.07 ? 0 : 1; }
        public DateTime WorkWeekDate { get => DateRef.GetWorkDate((decimal)(Cycle + 0.07 * WorkWeek + 0.06)); }
        public int WorkWeekDay { get => (int)((WorkDay % 1) * 100) % 7; }
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
        public string Client { get; set; }
        public string ClientCode { get; set; }
        public string Project { get; set; }
        public string BillType { get; set; }
        public string WorkType { get; set; }
        public string Contractor { get; set; }
    }
}

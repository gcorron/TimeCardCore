using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeCard.Helpers;

namespace TimeCard.Domain
{
    public class WorkSummary
    {
        public int JobId { get; set; }
        public string Descr { get; set; }
        public string shortDescr { get; set; }
        public decimal WorkPeriod { get; set; }
        public decimal Hours { get; set; }
        public string WorkPeriodDescr { get => $"{DateRef.PeriodEndDate(WorkPeriod): MM/dd/yyyy}"; }
    }
}

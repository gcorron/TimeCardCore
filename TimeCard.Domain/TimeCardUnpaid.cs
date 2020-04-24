using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeCard.Helpers;

namespace TimeCard.Domain
{
    public class TimeCardUnpaid
    {
        public decimal WorkDay { get; set; }
        public decimal Hours { get; set; }
        public override string ToString()
        {
            
            int cycle = (int)Decimal.Floor(WorkDay);
            if (cycle == 0)
            {
                return "- Select -";
            }
            else
            {
                return $"{ DateRef.PeriodEndDate(WorkDay):MM/dd/yyyy} {Hours:n2}";
            }
        }
    }
}

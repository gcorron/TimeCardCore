using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCard.Helpers
{
    public static class DateRef
    {
        const string BaselineDate = "12/22/2018";
        public static decimal GetWorkDay(DateTime? date)
        {
            if (date == null)
            {
                return 0;
            }

            DateTime refDate = DateTime.Parse(BaselineDate);

            if (date<refDate)
            {
                return 0;
            }

            decimal days = (decimal)((DateTime)date - refDate).TotalDays;
            decimal workDay = decimal.Floor(days / 14) + decimal.Remainder(days,14)/100;
            return workDay;
        }

        public static int CurrentWorkCycle
        {
            get
            {
                return (int)decimal.Floor(GetWorkDay(DateTime.Today));
            }
        }

        public static DateTime GetWorkDate(decimal workDay)
        {
            int cycle = (int)Decimal.Floor(workDay);
            DateTime refDate = DateTime.Parse(BaselineDate);
            return refDate.AddDays((double)(cycle * 14 + (workDay - cycle) * 100));
        }

        public static DateTime PeriodEndDate(decimal workDay)
        {
            int cycle = (int)Decimal.Floor(workDay);
            DateTime refDate = DateTime.Parse(BaselineDate);
            return refDate.AddDays((double)(cycle * 14 + 13));
        }

    }
}

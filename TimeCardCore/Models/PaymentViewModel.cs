using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using TimeCard.Domain;

namespace TimeCardCore.Models
{
    public class PaymentViewModel
    {
        private DateTime BaselineDate = new DateTime(2018, 12, 22);
        public IEnumerable<PaymentSummary> PaymentSummary { get; set; }

        public bool JobIsTimeCard { get; set; }
        public int SelectedContractorId { get; set; }
        public int SelectedJobId { get; set; }
        public Job SelectedJob { get; set; }
        public bool CanEdit { get => !(SelectedJobId == 0 || SelectedContractorId == 0); }
        public bool IsAdmin { get; set; }
        public Payment EditPayment { get; set; }
        public IEnumerable<Payment>Payments { get; set; }
        public IEnumerable<SelectListItem> TimeCardsUnpaid { get; set; }
        public bool AllHours { get; set; }
        public string WorkDate(decimal workDay)
        {
            if (workDay == 0 )
            {
                return null;
            }
            int cycle = (int)Decimal.Floor(workDay);
            return $"{BaselineDate.AddDays((double)(cycle * 14 + (workDay - cycle) * 100)): MM/dd/yyyy}";
        }
    }
}
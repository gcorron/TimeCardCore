using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeCard.Helpers;

namespace TimeCard.Domain
{
    public class PaymentSummary
    {
        public int JobId { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public string BillType { get; set; }
        public decimal Billed { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get => Billed - Paid; }
        public decimal StartDay { get; set; }
        public string StartDate { get => $"{DateRef.GetWorkDate(StartDay):MM/dd/yyyy}"; }
        public decimal PaidThruDay { get; set; }
        public string PaidThruDate { get
            {
                if (PaidThruDay == 0 )
                {
                    return "";
                }
                else
                {
                    return $"{DateRef.GetWorkDate(PaidThruDay):MM/dd/yyyy}";
                }
            }
        }
    }
}

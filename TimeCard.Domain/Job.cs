using System;
using TimeCard.Helpers;
namespace TimeCard.Domain
{
    public class Job
    {
        public int JobId { get; set; }
        public string Descr { get; set; }
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
        public int BillType { get; set; }
        public string BillTypeDescr { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public bool Active { get; set; }
        public decimal StartDay { get; set; }
        public DateTime? StartDate
        {
            get
            {
                if (StartDay == 0)
                {
                    return null;
                }
                return DateRef.GetWorkDate(StartDay);
            }

            set
            {
                StartDay = DateRef.GetWorkDay(value);
            }
        }
    }
}

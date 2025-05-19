
namespace TimeCard.Domain
{
    public class Budget
    {
        public int BudgetId {get;set;}
        public int JobId {get;set;}
        public int ContractorId { get; set; }
        public string UserFullName { get; set; }
        public int BudgetType { get; set; }
        public string Descr { get; set; }
        public decimal BudgetHours { get; set; }
        public decimal Worked { get; set; }
        public decimal Paid { get; set; }
        public bool Active { get; set; }
    }
}

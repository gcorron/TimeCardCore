using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeCard.Domain;

namespace TimeCardCore.Models
{
    public class BudgetViewModel
    {
        public IEnumerable<Budget> Budgets {get;set;}
        public Budget EditBudget { get; set; }
        public bool Active { get; set; }
        public IEnumerable<SelectListItem> Jobs { get; set; }
        public IEnumerable<SelectListItem> BudgetTypes { get; set; }
        public int ActionId { get; set; }
        public string Action { get; set; }
    }
}

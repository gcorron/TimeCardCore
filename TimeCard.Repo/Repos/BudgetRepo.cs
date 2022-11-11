using System;
using System.Collections.Generic;
using System.Text;
using TimeCard.Domain;

namespace TimeCard.Repo.Repos
{
    public class BudgetRepo: BaseRepo
    {
        public BudgetRepo(string connectionString) : base(connectionString)
        {
        }
        public IEnumerable<Budget> GetBudgets(bool active)
        {
            return QuerySp<Budget>("sBudget", new { active });
        }
        public void UpdateBudget(Budget budget)
        {
            ExecuteSp("uBudget", new { budget.BudgetId, budget.JobId, budget.BudgetType, budget.BudgetHours, budget.Active });
        }
    }
}

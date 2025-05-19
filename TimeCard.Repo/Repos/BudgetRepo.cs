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
        public IEnumerable<Budget> GetBudgets(bool active, int contractorId)
        {
            return QuerySp<Budget>("sBudget", new { active, contractorId });
        }
        public IEnumerable<Budget> GetBudgetsForJob(int jobId, int contractorId)
        {
            return QuerySp<Budget>("sBudget", new { jobId, contractorId });
        }

        public Budget GetBudget(int budgetId)
        {
            return QuerySingleSp<Budget>("sBudget", new { budgetId });
        }
        public void UpdateBudget(Budget budget)
        {
            ExecuteSp("uBudget", new { budget.BudgetId, budget.JobId, budget.BudgetType, budget.BudgetHours, budget.Active, budget.ContractorId });
        }
        public bool DeleteBudget(int budgetId)
        {
            return QuerySingleSp<bool>("dBudget", new { budgetId });
        }
    }
}

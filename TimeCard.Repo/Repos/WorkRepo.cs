using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TimeCard.Domain;

namespace TimeCard.Repo.Repos
{
    public class WorkRepo : BaseRepo
    {
        public WorkRepo(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Work> GetWork(int contractorId, decimal workDay, bool payCycle)
        {
            return QuerySp<Work>("sWork", new { contractorId, workDay, payCycle });
        }

        public void SaveWork(Work work)
        {
            ExecuteSp("uWork", new { work.WorkId, work.ContractorId, work.JobId, work.WorkDay, work.Descr, work.Hours, work.WorkType });
        }

        public void DeleteWork(int workId)
        {
            ExecuteSp("dWork", new { workId });
        }

        public IEnumerable<WorkExtended> GetWorkExtended(int contractorId, decimal workDay, bool payCycle)
        {
            return QuerySp<WorkExtended>("sWorkExtended", new { contractorId, workDay, payCycle });
        }

        public IEnumerable<WorkSummary> GetWorkSummary(int contractorId)
        {
            return QuerySp<WorkSummary>("sWorkSummary", new { contractorId });
        }

        public IEnumerable<Work> GetWorkJobDetail(int contractorId, int jobId)
        {
            return QuerySp<Work>("sWorkJobDetail", new { contractorId, jobId });
        }
        public IEnumerable<int> GetWorkOpen(int contractorId)
        {
            return QuerySp<int>("sWorkOpen", new { contractorId });
        }

        public void ToggleWorkOpen(int contractorId, int workCycle)
        {
            ExecuteSp("uWorkOpen", new { contractorId, workCycle });
        }

    }
}

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

        public IEnumerable<Lookup> GetJobs(string addFirstRow = null)
        {
            var data = QuerySp<Lookup>("sJobs", null);
            if (addFirstRow != null)
            {
                return new Lookup[] { new Lookup { Id = 0, Descr = addFirstRow } }.Union(data);
            }
            else
            {
                return data;
            }
        }
    }
}

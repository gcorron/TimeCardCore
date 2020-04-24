using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeCard.Domain;

namespace TimeCard.Repo.Repos
{
    public class JobRepo : BaseRepo
    {
        public JobRepo(string connectionString) : base(connectionString)
        {

        }

        public IEnumerable<Job> GetJobStart(int contractorId)
        {
            return QuerySp<Job>("sJobStart", new { contractorId });
        }

        public void UpdateJobStart(int contractorId, int jobId, decimal startDay, bool isNew)
        {
            ExecuteSp("uJobStart", new { contractorId, jobId, startDay, isNew });
        }

        public Job GetJob(int jobId)
        {
            return QuerySingleSp<Job>("sJob", new { jobId });
        }


    }
}

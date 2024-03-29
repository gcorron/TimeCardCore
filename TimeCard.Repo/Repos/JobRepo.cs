﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public IEnumerable<Job> GetJobStart(int contractorId, bool active = true)
        {
            return QuerySp<Job>("sJobStart", new { contractorId, active });
        }

        public void UpdateJobStart(int contractorId, int jobId, decimal startDay, bool isNew)
        {
            ExecuteSp("uJobStart", new { contractorId, jobId, startDay, isNew });
        }

        public Job GetJob(int jobId)
        {
            return QuerySingleSp<Job>("sJob", new { jobId });
        }

        public IEnumerable<Lookup> GetJobsForWork(int contractorId, decimal startDay)
        {
            string addFirstRow = "- Select -";
            var data = QuerySp<Lookup>("sJobsWork", new { contractorId, startDay });
            return new Lookup[] { new Lookup { Id = 0, Descr = addFirstRow } }.Union(data);
        }

        public IEnumerable<Lookup> GetJobsUnused()
        {
            return QuerySp<Lookup>("sJobsNotUsed",null);
        }

        public void SaveJob(int clientId, int projectId, int billType)
        {
            ExecuteSp("iJob", new { clientId, projectId, billType });
        }

        public void SaveJobDescr(int contractorId, int jobId, string descr)
        {
            ExecuteSp("uJobDescr", new { contractorId, jobId, descr });
        }

        public void DeleteJob(int jobId)
        {
            ExecuteSp("dJob", new { jobId });
        }
        public void Active(int jobId, bool active)
        {
            ExecuteSp("uJobActive", new { jobId, active });
        }
    }
}

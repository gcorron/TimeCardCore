using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeCard.Domain;

namespace TimeCard.Repo.Repos
{
    public class PaymentRepo : BaseRepo
    {
        public PaymentRepo(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<PaymentSummary> GetSummary(int contractorId, int beforeCycle)
        {
                return QuerySp<PaymentSummary>("sPaymentSummary", new { contractorId, beforeCycle });
        }

        public IEnumerable<Payment> GetPaymentsForJob(int contractorId, int jobId)
        {
            return QuerySp<Payment>("sPaymentsJob", new { contractorId, jobId });
        }

        public IEnumerable<Payment> GetPayments(int contractorId)
        {
            return QuerySp<Payment>("sPayments", new { contractorId });
        }

        public void SavePayment(Payment payment)
        {
            ExecuteSp("uPayment", new { payment.PayId, payment.ContractorId, payment.JobId, payment.Hours, payment.PayDate, payment.CheckNo, payment.WorkDay }) ;
        }

        public void DeletePayment(int payId)
        {
            ExecuteSp("dPayment", new { payId });
        }

        public IEnumerable<TimeCardUnpaid> GetJobTimeCardUnpaidCycles(int contractorId, int jobId)
        {
            return Enumerable.Repeat(new TimeCardUnpaid(),1).Union(QuerySp<TimeCardUnpaid>("sJobTimeCardUnpaidCycles",new { contractorId, jobId }));
        }

        public decimal GetJobPaidThruDate(int contractorId, int jobId)
        {
            return QuerySingleSp<decimal>("sJobPaidThruDate", new { contractorId, jobId });
        }

    }
}

using System.Collections.Generic;
using TimeCard.Domain;

namespace TimeCardCore.Models
{
    public class JobViewModel
    {
        public int ContractorId { get; set; }
        public IEnumerable<Job> Jobs { get; set; }
        public bool Active { get; set; }
        public int ChangeActiveJobId { get; set; }
    }
}

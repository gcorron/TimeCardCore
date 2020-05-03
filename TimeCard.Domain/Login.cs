using System;
using System.Collections.Generic;
using System.Text;

namespace TimeCard.Domain
{
    public class Login
    {
        public string Result { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int ContractorId { get; set; }
        public DateTime LoginTime { get; set; }
    }
}

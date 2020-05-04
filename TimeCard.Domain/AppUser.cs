using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimeCard.Domain
{
    public class AppUser
    {
        public int UserId { get; set; }
        [MaxLength(20)]
        public string UserName { get; set; }
        [MaxLength(50)]
        public string UserFullName { get; set; }
        public bool Active { get; set; }
        public bool Reset { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}

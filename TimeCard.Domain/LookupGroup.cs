using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimeCard.Domain
{
    public class LookupGroup
    {
        public int GroupId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Descr { get; set; }
    }
}

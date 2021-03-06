﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimeCard.Domain
{
    public class AppUser
    {
        public int UserId { get; set; }
        [MaxLength(20)]
        [Required]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserFullName { get; set; }
        public bool Active { get; set; }
        public bool Reset { get; set; }
        public DateTime? LastLogin { get; set; }
        public Lookup[] Roles { get; set; }
        public int ContractorId { get; set; }

    }
}

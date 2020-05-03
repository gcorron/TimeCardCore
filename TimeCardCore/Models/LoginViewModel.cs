using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeCardCore.Models
{
    public class LoginViewModel
    {
        [MaxLength(20)]
        [Required]
        public string UserName { get; set; }
        [MaxLength(20)]
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}

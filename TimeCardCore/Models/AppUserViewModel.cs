using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeCard.Domain;

namespace TimeCardCore.Models
{
    public class AppUserViewModel
    {
        public IEnumerable<AppUser> AppUsers { get; set; }
        public AppUser EditAppUser { get; set; }
    }
}

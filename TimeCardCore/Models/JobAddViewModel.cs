using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeCardCore.Models
{
    public class JobAddViewModel
    {
        public IEnumerable<SelectListItem> Clients {get;set;}
        public IEnumerable<SelectListItem> Projects { get; set; }
        public IEnumerable<SelectListItem> BillTypes { get; set; }
        public IEnumerable<SelectListItem> Jobs { get; set; }
        public int SelectedClientId { get; set; }
        public int SelectedProjectId { get; set; }
        public int SelectedBillTypeId { get; set; }
        public int SelectedJobId { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TimeCard.Domain;

namespace TimeCardCore.Models
{
    public class WorkViewModel
    {
        public TimeCard.Domain.Lookup Contractor { get; set; }
        public IEnumerable<SelectListItem> PayCycles { get; set; }
        public IEnumerable<SelectListItem> Jobs { get; set; }
        public IEnumerable<SelectListItem> WorkTypes { get; set; }
        public IEnumerable<SelectListItem> EditDays { get; set; }
        public decimal TestWorkDay { get; set; }
        public string TestToday { get; set; }
        public IEnumerable<Work> WorkEntries { get; set; }
        public int SelectedCycle { get; set; }
        public Work EditWork { get; set; }
    }
}
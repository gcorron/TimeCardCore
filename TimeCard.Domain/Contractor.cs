using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimeCard.Domain
{
    public class Contractor
    {
        public int ContractorId { get; set; }
        [Required]
        [MaxLength(50)]
        public string InvoiceName { get; set; }
        [Required]
        [MaxLength(250)]
        public string InvoiceAddress { get; set; }
        [Required]
        [Range(minimum:0, maximum:999)]
        public decimal Rate { get; set; }
    }
}

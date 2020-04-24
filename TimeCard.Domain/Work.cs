﻿using System;
using System.ComponentModel.DataAnnotations;
using TimeCard.Helpers;

namespace TimeCard.Domain
{
    public class Work
    {
        public int WorkId { get; set; }
        [Range(1,int.MaxValue,ErrorMessage = "Contractor ID required")]
        public int ContractorId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Job")]
        public int JobId { get; set; }
        public string Job { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Work Type")]
        public int WorkType { get; set; }
        public string WorkTypeDescr { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Date")]
        public decimal WorkDay { get; set; }
        [Required]
        public string Descr { get; set; }
        [Range(0.25, 8, ErrorMessage = "Hours Not Valid")]
        public decimal Hours { get; set; }
        public string WorkDate
        {
            get
            {
                return $"{DateRef.GetWorkDate(WorkDay):MM/dd}";
            }
        }
    }
}

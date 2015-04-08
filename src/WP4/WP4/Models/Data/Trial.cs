using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WP4.Models.Data
{
    public class Trial : VersionControl
    {
        [Required]
        public int RetrievalTeam_ID { get; set; }
        [ForeignKey("RetrievalTeam_ID")]
        public virtual RetrievalTeam RetrievalTeam { get; set; }

        [Display(Name="a Maastricht category III DCD donor")]
        public bool CriteriaCheck1 { get; set; }

        /// <summary>
        /// Can be automatically set based on answers in the system
        /// </summary>
        [Display(Name="aged 50 years or older")]
        public bool CriteriaCheck2 { get; set; }

        /// <summary>
        /// Can be automatically set based on answers in the system
        /// </summary>
        [Display(Name="with both kidneys registered for donation")]
        public bool CriteriaCheck3 { get; set; }

        [Display(Name="from the collaborating donor regions reported to Eurotransplant (ET) / National Health Service Blood and Transplant (NHSBT)")]
        public bool CriteriaCheck4 { get; set; }

        /// <summary>
        /// Can be automatically set based on answers in the system
        /// </summary>
        [Display(Name="")]
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Generated from data "WP4" + RetrievalTeam.CentreCode + Trial.ID(padded to three numbers)
        /// </summary>
        [Display(Name="")]
        public string TrialID { get; set; } 


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WP4.Models.Data
{
    public class Donor : VersionControl
    {
        [Required]
        [Display(Name="ET Donor number/ NHSBT Number")]
        public string Number { get; set; }

        public int Age { get; set; } // or DoB
        public DateTime DoB { get; set; }  // Not Known

        [Required, Display(Name="Date of admission")]
        public DateTime Admission { get; set; }

        [Required, Display(Name="Admitted to ITU")]
        public bool AdmittedToITU { get; set; }

        [Display(Name="Date admitted to ITU")]
        public DateTime DateAdmitedToITU { get; set; }

        [Required, Display(Name="Date of procurement")]
        public DateTime DateOfProcurement { get; set; }

        [Required, Display(Name="Gender")]
        public Gender Gender { get; set; }

        [Display(Name="Ethnicity")]
        public Ethnicity Ethnicity { get; set; }

        [Required, Display(Name="Weight (kg)"), Range(20,200)]
        public int Weight { get; set; }

        [Required, Display(Name="Height (cm)"), Range(100,250)]
        public int Height { get; set; }

        [NotMapped, Display(Name="BMI Derived Value")]
        public int BMI { get; set; }

        [Display(Name="Blood group")]
        public BloodGroup BloodGroup { get; set; }

        [Display(Name="Other organs offered for donation"), InverseProperty("Donor")]
        public virtual ICollection<OrgansOffered> OrgansOfferedForDonation { get; set; }

        // Nav link to the Organ(s) in the trial
    }
}
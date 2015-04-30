using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WP4.Models.Data
{
    public class OrgansOffered
    {
        [Key, Required]
        public OrgansForDonation Organ { get; set; }

        [Key, Required]
        public int Donor_ID { get; set; }
        [ForeignKey("Donor_ID")]
        public virtual Donor Donor { get; set; }
    }
}
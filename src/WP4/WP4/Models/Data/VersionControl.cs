using System;
using System.ComponentModel.DataAnnotations;

namespace WP4.Models.Data
{
    public abstract class VersionControl
    {
        [Required, Key] 
        public int ID { get; set; } // Primary Key

        [Required]
        public int Version { get; set; } // Maintained by the system

        [Required]
        public DateTime CreatedOn { get; set; }  // Set by the system

        [Required]
        public User CreatedBy { get; set; }

        // Concurrency checking - remember to handle the DbUpdateConcurrencyException if triggered
        // https://msdn.microsoft.com/en-gb/data/jj591583.aspx#TimeStamp
        [Timestamp] 
        public Byte[] TimeStamp { get; set; }
    }
}
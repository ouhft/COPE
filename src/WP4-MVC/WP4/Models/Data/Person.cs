using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WP4.Models.Data
{
    public class Person : VersionControl
    {
        [Required]
        public string FirstNames { get; set; }

        [Required]
        public string LastNames { get; set; }

        [NotMapped, NonSerialized]
        public string FullName
        {
            get
            {
                return this.FirstNames + " " + this.LastNames;
            }
        }

        //public Nullable<int> User_ID { get; set; } -- Not sure what the reference will be yet
        //[ForeignKey("User_ID")]
        public virtual ApplicationUser User { get; set; }
    }
}
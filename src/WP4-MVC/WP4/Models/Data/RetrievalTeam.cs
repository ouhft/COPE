using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WP4.Models.Data
{
    /// <summary>
    /// Lookup table. Admin editable.
    /// </summary>
    public class RetrievalTeam
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Country { get; set; }

        [Required, Range(10,99), Index]
        public int CentreCode { get; set; }
    }
}
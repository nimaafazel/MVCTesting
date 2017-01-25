using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EntityFUnit.Models
{
    public class Person
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]       
        public int id { get; set; }

        [Required]
        public string name { get; set; }
        public string lastName { get; set; }

        [Required]
        public int? age { get; set; }
    }
}
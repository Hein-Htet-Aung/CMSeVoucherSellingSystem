using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCodeTest.Models
{
    public class Listing
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Code { get; set; }
        public string Item { get; set; }
        public string Descr { get; set; }
        public string Status { get; set; }
        public string Usrdef01 { get; set; }
        [Required]
        public string Email { get; set; }
    }
}

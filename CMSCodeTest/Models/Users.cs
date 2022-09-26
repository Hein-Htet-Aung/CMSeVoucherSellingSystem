using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCodeTest.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public Int64 User_ID { get; set; }
        public string User_Name { get; set; }
        public string Phone_Number { get; set; }
        public string Password { get; set; }
        public string User_Role { get; set; }
        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Account_Status { get; set; }
        public string Last_Login { get; set; }
        public string Create_By { get; set; }
        public string Create_Date { get; set; }
        public string Update_By { get; set; }
        public string Update_Date { get; set; }
        public string IsDeleted { get; set; }
    }
}

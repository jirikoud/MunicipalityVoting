using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VotingCoreData.Models
{
    [Table("AspNetUserRoles")]
    public class UserRole
    {
        public string UserId { get; set; }

        public string RoleId { get; set; }

        public User User {get;set;}
        public Role Role {get;set;}
    }
}

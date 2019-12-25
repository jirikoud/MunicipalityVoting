using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VotingCoreData.Models
{
    [Table("AspNetRoles")]
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}

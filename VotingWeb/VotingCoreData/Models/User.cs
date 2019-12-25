using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using VotingCommon;
using VotingCommon.Converts;

namespace VotingCoreData.Models
{
    [Table("AspNetUsers")]
    public class User
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public List<UserClaim> Claims { get; set; }

        public string GetRoleFormat()
        {
            var roles = this.UserRoles.Select(item => RoleConvert.Decode(item.Role.Name)).Where(item => item != null);
            return string.Join(", ", roles);
        }

        public string GetMunicipalityFormat(List<Municipality> municipalities)
        {
            var municipalityIds = this.Claims.Where(item => item.ClaimType == Constants.CLAIM_MUNICIPALITY).Select(item => int.Parse(item.ClaimValue));
            var list = municipalities.Where(item => municipalityIds.Contains(item.Id)).Select(item => item.Name);
            return string.Join(", ", list);
        }
    }
}

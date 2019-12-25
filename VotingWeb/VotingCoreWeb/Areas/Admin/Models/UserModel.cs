using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        [Display(Name = "DETAIL_USER_NAME", ResourceType = typeof(UserAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string UserName { get; set; }

        [Display(Name = "DETAIL_ROLE", ResourceType = typeof(UserAdminRes))]
        public string Role { get; set; }

        [Display(Name = "DETAIL_MUNICIPALITY", ResourceType = typeof(UserAdminRes))]
        public int MunicipalityId { get; set; }

        [StringLength(100, ErrorMessageResourceName = "VALIDATION_PASSWORD_LENGTH", MinimumLength = 6, ErrorMessageResourceType = typeof(UserAdminRes))]
        [DataType(DataType.Password)]
        [Display(Name = "DETAIL_PASSWORD", ResourceType = typeof(UserAdminRes))]
        public string Password { get; set; }

        public UserModel()
        {
        }

        public UserModel(IdentityUser user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
        }
    }
}

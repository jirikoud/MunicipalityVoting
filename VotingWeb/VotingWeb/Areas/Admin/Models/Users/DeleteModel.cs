using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Users
{
    public class DeleteModel
    {
        public string Id { get; set; }

        [Display(Name = "DETAIL_USER_NAME", ResourceType = typeof(UserAdminRes))]
        public string UserName { get; set; }

        public DeleteModel()
        {
        }

        public DeleteModel(IdentityUser entity)
        {
            this.Id = entity.Id;
            this.UserName = entity.UserName;
        }
    }
}
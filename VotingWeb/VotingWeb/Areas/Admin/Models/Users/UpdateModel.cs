using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingData;
using VotingWeb.DataContext;
using VotingWeb.Infrastructure;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Users
{
    public class UpdateModel
    {
        public string Id { get; set; }
        public bool IsCreate { get; set; }

        [Display(Name = "DETAIL_USER_NAME", ResourceType = typeof(UserAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string UserName { get; set; }

        [Display(Name = "DETAIL_ROLE", ResourceType = typeof(UserAdminRes))]
        public string Role { get; set; }
        public SelectList RoleList { get; set; }

        [Display(Name = "DETAIL_MUNICIPALITY", ResourceType = typeof(UserAdminRes))]
        public int Municipality { get; set; }
        public SelectList MunicipalityList { get; set; }

        [StringLength(100, ErrorMessageResourceName = "VALIDATION_PASSWORD_LENGTH", MinimumLength = 6, ErrorMessageResourceType = typeof(UserAdminRes))]
        [DataType(DataType.Password)]
        [Display(Name = "DETAIL_PASSWORD", ResourceType = typeof(UserAdminRes))]
        public string Password { get; set; }

        public UpdateModel()
        {
        }

        public UpdateModel(ApplicationUserManager userManager, IdentityUser user)
        {
            if (user != null)
            {
                this.Id = user.Id;
                this.UserName = user.UserName;
                if (userManager.IsInRole(user.Id, Infrastructure.Constants.ROLE_EDITOR))
                {
                    this.Role = Infrastructure.Constants.ROLE_EDITOR;
                }
                if (userManager.IsInRole(user.Id, Infrastructure.Constants.ROLE_ADMIN))
                {
                    this.Role = Infrastructure.Constants.ROLE_ADMIN;
                }
                var claim = user.Claims.FirstOrDefault(item => item.ClaimType == Infrastructure.Constants.CLAIM_MUNICIPALITY);
                if (claim != null)
                {
                    this.Municipality = int.Parse(claim.ClaimValue);
                }
            }
            else
            {
                this.IsCreate = true;
            }
        }

        public void FillSelectLists(VotingEntities entities)
        {
            this.RoleList = new SelectList(RoleConvert.GetRoleList(), "Item1", "Item2", this.Role);
            var municipalityList = MunicipalityContext.Instance.LoadList(entities).ConvertAll(item => new Tuple<int, string>(item.Id, item.Name));
            municipalityList.Insert(0, new Tuple<int, string>(-1, null));
            this.MunicipalityList = new SelectList(municipalityList, "Item1", "Item2", this.Municipality);
        }

        public void Validate(ModelStateDictionary modelState)
        {
            if (this.IsCreate && string.IsNullOrWhiteSpace(this.Password))
            {
                modelState.AddModelError("Password", UserAdminRes.VALIDATION_REQUIRED_PASSWORD);
            }
        }
    }
}
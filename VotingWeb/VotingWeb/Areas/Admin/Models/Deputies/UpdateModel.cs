using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Deputies
{
    public class UpdateModel
    {
        public int Id { get; set; }
        public bool IsCreate { get; set; }
        public int MunicipalityId { get; set; }

        [Display(Name = "DETAIL_FIRSTNAME", ResourceType = typeof(DeputyAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string Firstname { get; set; }

        [Display(Name = "DETAIL_LASTNAME", ResourceType = typeof(DeputyAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string Lastname { get; set; }

        [Display(Name = "DETAIL_TITLE_PRE", ResourceType = typeof(DeputyAdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string TitlePre { get; set; }

        [Display(Name = "DETAIL_TITLE_POST", ResourceType = typeof(DeputyAdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string TitlePost { get; set; }

        public UpdateModel()
        {
        }

        public UpdateModel(Deputy entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.MunicipalityId = entity.MunicipalityId;
                this.Firstname = entity.Firstname;
                this.Lastname = entity.Lastname;
                this.TitlePre = entity.TitlePre;
                this.TitlePost = entity.TitlePost;
            }
            else
            {
                this.IsCreate = true;
            }
        }
    }
}
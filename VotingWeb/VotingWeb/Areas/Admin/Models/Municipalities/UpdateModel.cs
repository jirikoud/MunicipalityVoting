using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Municipalities
{
    public class UpdateModel
    {
        public int Id { get; set; }
        public bool IsCreate { get; set; }

        [Display(Name = "DETAIL_NAME", ResourceType = typeof(MunicipalityAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string Name { get; set; }

        [Display(Name = "DETAIL_DESCRIPTION", ResourceType = typeof(MunicipalityAdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string Description { get; set; }

        public UpdateModel()
        {
        }

        public UpdateModel(Municipality entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.Name = entity.Name;
                this.Description = entity.Description;
            }
            else
            {
                this.IsCreate = true;
            }
        }
    }
}
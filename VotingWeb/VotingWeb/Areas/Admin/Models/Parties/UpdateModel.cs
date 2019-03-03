using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Parties
{
    public class UpdateModel
    {
        public int Id { get; set; }
        public bool IsCreate { get; set; }
        public int MunicipalityId { get; set; }

        [Display(Name = "DETAIL_NAME", ResourceType = typeof(PartyAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string Name { get; set; }

        public UpdateModel()
        {
        }

        public UpdateModel(Party entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.MunicipalityId = entity.MunicipalityId;
                this.Name = entity.Name;
            }
            else
            {
                this.IsCreate = true;
            }
        }
    }
}
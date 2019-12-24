using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VotingCoreData.Properties;

namespace VotingCoreData.Models
{
    public class Municipality
    {
        public int Id { get; set; }

        [Display(Name = "DETAIL_NAME", ResourceType = typeof(MunicipalityRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Name { get; set; }

        [Display(Name = "DETAIL_DESCRIPTION", ResourceType = typeof(MunicipalityRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public List<Deputy> Deputies { get; set; }
        public List<Party> Parties { get; set; }
        public List<Session> Sessions { get; set; }

        public void UpdateFrom(Municipality model)
        {
            this.Name = model.Name;
            this.Description = model.Description;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VotingCoreData.Properties;

namespace VotingCoreData.Models
{
    public class Body
    {
        public int Id { get; set; }

        [Display(Name = "BODY_NAME", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
        public string Name { get; set; }

        [Display(Name = "BODY_DESCRIPTION", ResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
        public string Description { get; set; }

        public int MunicipalityId { get; set; }
        public bool IsDeleted { get; set; }

        public Municipality Municipality { get; set; }
        public List<Session> Sessions { get; set; }

        public List<BodyMember> BodyMembers { get; set; }

        public void UpdateFrom(Body model)
        {
            this.Name = model.Name;
            this.Description = model.Description;
        }
    }
}

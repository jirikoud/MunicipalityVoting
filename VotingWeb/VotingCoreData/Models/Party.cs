using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VotingCoreData.Properties;

namespace VotingCoreData.Models
{
    public class Party
    {
        public int Id { get; set; }
        public int MunicipalityId { get; set; }

        [Display(Name = "DETAIL_NAME", ResourceType = typeof(PartyRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public Municipality Municipality { get; set; }
        public List<Voting> Votings { get; set; }

        public void UpdateFrom(Party model)
        {
            this.Name = model.Name;
        }
    }
}

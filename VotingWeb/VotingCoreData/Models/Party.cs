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

        [Display(Name = "PARTY_NAME", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
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

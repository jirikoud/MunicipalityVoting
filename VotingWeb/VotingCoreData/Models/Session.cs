using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VotingCoreData.Properties;

namespace VotingCoreData.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Display(Name = "DETAIL_NAME", ResourceType = typeof(SessionRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        
        public string Name { get; set; }
        [Display(Name = "DETAIL_CHAIRMAN", ResourceType = typeof(SessionRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        
        public string Chairman { get; set; }
        [Display(Name = "DETAIL_DATE_START", ResourceType = typeof(SessionRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ValidationRes))]
        
        public DateTime? StartDate { get; set; }
        [Display(Name = "DETAIL_DATE_END", ResourceType = typeof(SessionRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ValidationRes))]
        public DateTime? EndDate { get; set; }

        public int MunicipalityId { get; set; }
        public bool IsDeleted { get; set; }

        public Municipality Municipality { get; set; }
        public List<Topic> Topics { get; set; }

        public void UpdateFrom(Session model)
        {
            this.Name = model.Name;
            this.Chairman = model.Chairman;
            this.StartDate = model.StartDate;
            this.EndDate = model.EndDate;
        }

        public string StartDateFormat()
        {
            return this.StartDate?.ToString("dd.MM.yyyy");
        }

    }
}

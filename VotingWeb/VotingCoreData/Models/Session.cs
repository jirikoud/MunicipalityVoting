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

        [Display(Name = "SESSION_NAME", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
        public string Name { get; set; }

        [Display(Name = "SESSION_CHAIRMAN", ResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
        public string Chairman { get; set; }

        [Display(Name = "SESSION_DATE_START", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
        public DateTime? StartDate { get; set; }

        [Display(Name = "SESSION_DATE_END", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
        public DateTime? EndDate { get; set; }

        public int BodyId { get; set; }
        public bool IsDeleted { get; set; }

        public Body Body { get; set; }
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

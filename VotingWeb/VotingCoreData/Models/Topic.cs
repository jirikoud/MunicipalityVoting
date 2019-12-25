using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VotingCoreData.Properties;

namespace VotingCoreData.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public int SessionId { get; set; }

        [Display(Name = "DETAIL_NAME", ResourceType = typeof(TopicRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ValidationRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Name { get; set; }

        [Display(Name = "DETAIL_COMMENT", ResourceType = typeof(TopicRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ValidationRes))]
        public string Comment { get; set; }

        [Display(Name = "DETAIL_ORDER", ResourceType = typeof(TopicRes))]
        public int Order { get; set; }

        [Display(Name = "DETAIL_IS_PROCEDURAL", ResourceType = typeof(TopicRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ValidationRes))]
        public bool IsProcedural { get; set; }

        [Display(Name = "DETAIL_IS_SECRET", ResourceType = typeof(TopicRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ValidationRes))]
        public bool IsSecret { get; set; }

        public DateTime? Time { get; set; }
        public int? Total { get; set; }
        public bool IsDeleted { get; set; }

        public Session Session { get; set; }
        public List<Shortcut> Shortcuts { get; set; }
        public List<Voting> Votings { get; set; }

        public void UpdateFrom(Topic model)
        {
            this.Name = model.Name;
            this.Comment = model.Comment;
            this.Order = model.Order;
            this.IsProcedural = model.IsProcedural;
            this.IsSecret = model.IsSecret;
        }
    }
}

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

        [Display(Name = "TOPIC_NAME", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
        public string Name { get; set; }

        [Display(Name = "TOPIC_COMMENT", ResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
        public string Comment { get; set; }

        [Display(Name = "TOPIC_TEXT", ResourceType = typeof(ModelRes))]
        public string Text { get; set; }

        [Display(Name = "TOPIC_ORDER", ResourceType = typeof(ModelRes))]
        public int Order { get; set; }

        [Display(Name = "TOPIC_IS_PROCEDURAL", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
        public bool IsProcedural { get; set; }

        [Display(Name = "TOPIC_IS_SECRET", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
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
            this.Text = model.Text;
            this.Order = model.Order;
            this.IsProcedural = model.IsProcedural;
            this.IsSecret = model.IsSecret;
        }
    }
}

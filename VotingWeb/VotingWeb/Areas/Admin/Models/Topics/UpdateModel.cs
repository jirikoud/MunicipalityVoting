using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Topics
{
    public class UpdateModel
    {
        public int Id { get; set; }
        public bool IsCreate { get; set; }
        public int SessionId { get; set; }

        [Display(Name = "DETAIL_NAME", ResourceType = typeof(TopicAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string Name { get; set; }

        [Display(Name = "DETAIL_COMMENT", ResourceType = typeof(TopicAdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string Comment { get; set; }

        [Display(Name = "DETAIL_ORDER", ResourceType = typeof(TopicAdminRes))]
        public int Order { get; set; }

        [Display(Name = "DETAIL_IS_PROCEDURAL", ResourceType = typeof(TopicAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        public bool IsProcedural { get; set; }

        [Display(Name = "DETAIL_IS_SECRET", ResourceType = typeof(TopicAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        public bool IsSecret { get; set; }

        public UpdateModel()
        {
        }

        public UpdateModel(Topic entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.SessionId = entity.SessionId;
                this.Order = entity.Order;
                this.Name = entity.Name;
                this.Comment = entity.Comment;
                this.IsProcedural = entity.IsProcedural;
                this.IsSecret = entity.IsSecret;
            }
            else
            {
                this.IsCreate = true;
            }
        }
    }
}
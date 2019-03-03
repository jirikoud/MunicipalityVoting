using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Topics
{
    public class DeleteModel
    {
        public int Id { get; set; }
        public int SessionId { get; set; }

        [Display(Name = "DETAIL_NAME", ResourceType = typeof(TopicAdminRes))]
        public string Name { get; set; }

        public DeleteModel()
        {
        }

        public DeleteModel(Topic entity)
        {
            this.Id = entity.Id;
            this.SessionId = entity.SessionId;
            this.Name = entity.Name;
        }
    }
}
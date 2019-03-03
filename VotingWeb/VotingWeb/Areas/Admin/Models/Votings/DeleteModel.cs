using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Votings
{
    public class DeleteModel
    {
        public int Id { get; set; }
        public int TopicId { get; set; }

        [Display(Name = "DETAIL_DEPUTY", ResourceType = typeof(VotingAdminRes))]
        public string DeputyName { get; set; }

        public DeleteModel()
        {
        }

        public DeleteModel(Voting entity)
        {
            this.Id = entity.Id;
            this.TopicId = entity.TopicId;
            this.DeputyName = entity.Deputy.GetFullName();
        }
    }
}
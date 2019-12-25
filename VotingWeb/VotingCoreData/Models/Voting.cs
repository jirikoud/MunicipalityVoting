using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VotingCommon.Converts;
using VotingCommon.Enumerations;
using VotingCoreData.Properties;

namespace VotingCoreData.Models
{
    public class Voting
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        [Display(Name = "DETAIL_DEPUTY", ResourceType = typeof(VotingRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ValidationRes))]
        public int DeputyId { get; set; }
        [Display(Name = "DETAIL_PARTY", ResourceType = typeof(VotingRes))]
        public int? PartyId { get; set; }
        [Display(Name = "DETAIL_VOTE", ResourceType = typeof(VotingRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ValidationRes))]
        public int Vote { get; set; }

        public Deputy Deputy { get; set; }
        public Party Party { get; set; }
        public Topic Topic { get; set; }

        public void UpdateFrom(Voting model)
        {
            this.DeputyId = model.DeputyId;
            this.PartyId = model.PartyId;
            this.Vote = model.Vote;
        }

        public string GetVoteFormat()
        {
            return VoteConvert.Convert((VoteEnum)this.Vote);
        }
    }
}

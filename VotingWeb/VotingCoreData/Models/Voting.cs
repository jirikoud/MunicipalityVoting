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
        [Display(Name = "VOTING_DEPUTY", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
        public int DeputyId { get; set; }
        [Display(Name = "VOTING_PARTY", ResourceType = typeof(ModelRes))]
        public int? PartyId { get; set; }
        [Display(Name = "VOTING_VOTE", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
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

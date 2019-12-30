using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingCommon.Enumerations;
using VotingCoreData.Models;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Models.Session
{
    public class TopicItem
    {
        public int Id { get; set; }
        public string Order { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        public bool IsShowResults { get; set; }
        public int VoteYes { get; set; }
        public int VoteNo { get; set; }
        public int VoteAbstain { get; set; }
        public int VoteMissing { get; set; }
        public int VoteNot { get; set; }

        public TopicItem(VotingCoreData.Models.Topic entity)
        {
            this.Id = entity.Id;
            this.Order = entity.Order.ToString();
            this.Name = entity.Name;
            this.Comment = entity.Comment;
            this.IsApproved = entity.IsApproved;
            if (!entity.IsSecret)
            {
                this.IsShowResults = true;
                this.VoteYes = entity.Votings.Count(item => item.Vote == (int)VoteEnum.Yes);
                this.VoteNo = entity.Votings.Count(item => item.Vote == (int)VoteEnum.No);
                this.VoteAbstain = entity.Votings.Count(item => item.Vote == (int)VoteEnum.Abstain);
                this.VoteMissing = entity.Votings.Count(item => item.Vote == (int)VoteEnum.Missing);
                this.VoteNot = entity.Votings.Count(item => item.Vote == (int)VoteEnum.NotVoting);
            }
        }
    }
}

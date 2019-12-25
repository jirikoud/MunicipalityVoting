using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingCommon.Converts;
using VotingCommon.Enumerations;
using VotingCoreData.Models;

namespace VotingCoreWeb.Models.Topic
{
    public class VoteOptionModel
    {
        public VoteEnum Vote { get; set; }
        public string OptionName { get; set; }
        public List<VoteModel> VoteList { get; set; }

        public VoteOptionModel(VoteEnum vote, List<Voting> VoteList)
        {
            this.Vote = vote;
            this.OptionName = VoteConvert.Convert(vote);
            this.VoteList = VoteList.ConvertAll(item => new VoteModel(item));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingCommon.Enumerations;
using VotingCoreData.Models;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Models.Topic
{
    public class PartyModel
    {
        public string Name { get; set; }
        public List<VoteOptionModel> VoteOptionList { get; set; }

        public PartyModel(List<Voting> votingList)
        {
            var party = votingList[0].Party;
            if (party != null)
            {
                this.Name = party.Name;
            }
            else
            {
                this.Name = VotingRes.PARTY_NONE;
            }

            var voteOptionDictionary = new Dictionary<VoteEnum, List<Voting>>();
            foreach (var voting in votingList)
            {
                var vote = (VoteEnum)voting.Vote;
                if (!voteOptionDictionary.ContainsKey(vote))
                {
                    voteOptionDictionary.Add(vote, new List<Voting>());
                }
                voteOptionDictionary[vote].Add(voting);
            }

            this.VoteOptionList = new List<VoteOptionModel>();
            foreach (var keyValue in voteOptionDictionary)
            {
                this.VoteOptionList.Add(new VoteOptionModel(keyValue.Key, keyValue.Value));
            }
            this.VoteOptionList.Sort((item1, item2) => item1.Vote.CompareTo(item2.Vote));
        }
    }
}
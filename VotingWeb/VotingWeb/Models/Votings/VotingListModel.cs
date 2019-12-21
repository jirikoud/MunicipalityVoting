﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingCommon.Enumerations;
using VotingData;
using VotingWeb.Infrastructure;

namespace VotingWeb.Models.Votings
{
    public class VotingListModel
    {
        private const int NO_PARTY = -1;

        public int MunicipalityId { get; set; }
        public string MunicipalityName { get; set; }
        public int SessionId { get; set; }
        public string SessionName { get; set; }
        public int TopicId { get; set; }
        public int TopicOrder { get; set; }
        public string TopicName { get; set; }
        public string TopicShortname { get; set; }
        public string TopicComment { get; set; }

        public List<VoteOptionModel> VoteOptionList { get; set; }

        public List<PartyModel> PartyList { get; set; }

        public VotingListModel(Topic topic, List<Voting> votingList)
        {
            this.MunicipalityId = topic.Session.MunicipalityId;
            this.MunicipalityName = topic.Session.Municipality.Name;
            this.SessionId = topic.Session.Id;
            this.SessionName = topic.Session.Name;
            this.TopicId = topic.Id;
            this.TopicOrder = topic.Order;
            this.TopicName = topic.Name;
            this.TopicShortname = ContextUtils.Instance.ShortenString(topic.Name, 40);
            this.TopicComment = topic.Comment;

            var voteOptionDictionary = new Dictionary<VoteEnum, List<Voting>>();
            var partyDictionary = new Dictionary<int, List<Voting>>();
            foreach (var voting in votingList)
            {
                var vote = (VoteEnum)voting.Vote;
                if (!voteOptionDictionary.ContainsKey(vote))
                {
                    voteOptionDictionary.Add(vote, new List<Voting>());
                }
                voteOptionDictionary[vote].Add(voting);

                var partyId = voting.PartyId ?? NO_PARTY;
                if (!partyDictionary.ContainsKey(partyId))
                {
                    partyDictionary.Add(partyId, new List<Voting>());
                }
                partyDictionary[partyId].Add(voting);
            }

            this.VoteOptionList = new List<VoteOptionModel>();
            foreach (var keyValue in voteOptionDictionary)
            {
                this.VoteOptionList.Add(new VoteOptionModel(keyValue.Key, keyValue.Value));
            }
            this.VoteOptionList.Sort((item1, item2) => item1.Vote.CompareTo(item2.Vote));

            this.PartyList = new List<PartyModel>();
            foreach (var partyVoteList in partyDictionary.Values)
            {
                this.PartyList.Add(new PartyModel(partyVoteList));
            }
            this.PartyList.Sort((item1, item2) => item1.Name.CompareTo(item2.Name));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingCommon.Enumerations;
using VotingData;
using VotingWeb.Infrastructure;

namespace VotingWeb.Models.Votings
{
    public class VoteModel
    {
        public string DeputyName { get; set; }
        public string Party { get; set; }
        public string ItemClass { get; set; }

        public VoteModel(Voting entity)
        {
            this.DeputyName = entity.Deputy.GetFullName();
            if (entity.Party != null)
            {
                this.Party = entity.Party.Name;
            }
            this.ItemClass = VoteConvert.GetItemClass((VoteEnum)entity.Vote);
        }
    }
}
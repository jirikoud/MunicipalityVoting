using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Models.Api
{
    public class Voting
    {
        public int? Id { get; set; }
        public int TopicId { get; set; }
        public int DeputyId { get; set; }
        public int? PartyId { get; set; }
        public int Vote { get; set; }

        public Voting() { }

        public Voting(VotingCoreData.Models.Voting model)
        {
            this.Id = model.Id;
            this.TopicId = model.TopicId;
            this.DeputyId = model.DeputyId;
            this.PartyId = model.PartyId;
            this.Vote = model.Vote;
        }

        public VotingCoreData.Models.Voting ToDbModel()
        {
            var model = new VotingCoreData.Models.Voting()
            {
                Id = this.Id ?? 0,
                TopicId = this.TopicId,
                DeputyId = this.DeputyId,
                PartyId = this.PartyId,
                Vote = this.Vote,
            };
            return model;
        }
    }
}

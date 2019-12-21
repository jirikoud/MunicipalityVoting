using System;
using System.Collections.Generic;
using System.Text;

namespace VotingCoreData.Models
{
    public class Voting
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public int DeputyId { get; set; }
        public int? PartyId { get; set; }
        public int Vote { get; set; }

        public Deputy Deputy { get; set; }
        public Party Party { get; set; }
        public Topic Topic { get; set; }

    }
}

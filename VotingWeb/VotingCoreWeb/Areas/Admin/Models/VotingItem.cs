using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingCoreData.Models;

namespace VotingCoreWeb.Areas.Admin.Models
{
    public class VotingItem
    {
        public int? PartyId { get; set; }
        public int DeputyId { get; set; }
        public string DeputyName { get; set; }
        public int Vote { get; set; }

        public VotingItem()
        {

        }

        public VotingItem(Deputy deputy, int vote)
        {
            this.PartyId = deputy.PartyId;
            this.DeputyId = deputy.Id;
            this.DeputyName = deputy.GetFullName();
            this.Vote = vote;
        }
    }
}

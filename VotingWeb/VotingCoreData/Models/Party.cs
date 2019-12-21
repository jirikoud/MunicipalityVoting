using System;
using System.Collections.Generic;
using System.Text;

namespace VotingCoreData.Models
{
    public class Party
    {
        public int Id { get; set; }
        public int MunicipalityId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public Municipality Municipality { get; set; }
        public List<Voting> Votings { get; set; }
    }
}

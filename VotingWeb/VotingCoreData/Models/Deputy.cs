using System;
using System.Collections.Generic;
using System.Text;

namespace VotingCoreData.Models
{
    public class Deputy
    {
        public int Id { get; set; }
        public int MunicipalityId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsDeleted { get; set; }
        public string TitlePre { get; set; }
        public string TitlePost { get; set; }

        public Municipality Municipality { get; set; }
        public List<Voting> Votings { get; set; }
    }
}

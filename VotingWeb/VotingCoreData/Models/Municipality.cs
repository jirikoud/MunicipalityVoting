using System;
using System.Collections.Generic;
using System.Text;

namespace VotingCoreData.Models
{
    public class Municipality
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public List<Deputy> Deputies { get; set; }
        public List<Party> Parties { get; set; }
        public List<Session> Sessions { get; set; }

    }
}

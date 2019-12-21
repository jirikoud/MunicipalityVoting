using System;
using System.Collections.Generic;
using System.Text;

namespace VotingCoreData.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int Order { get; set; }
        public DateTime? Time { get; set; }
        public int? Total { get; set; }
        public bool IsProcedural { get; set; }
        public bool IsSecret { get; set; }
        public bool IsDeleted { get; set; }

        public Session Session { get; set; }
        public List<Shortcut> Shortcuts { get; set; }
        public List<Voting> Votings { get; set; }
    }
}

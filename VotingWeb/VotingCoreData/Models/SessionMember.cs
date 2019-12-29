using System;
using System.Collections.Generic;
using System.Text;

namespace VotingCoreData.Models
{
    public class SessionMember
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int DeputyId { get; set; }

        public Session Session { get; set; }
        public Deputy Deputy { get; set; }
    }
}

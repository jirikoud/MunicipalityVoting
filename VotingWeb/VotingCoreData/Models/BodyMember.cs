using System;
using System.Collections.Generic;
using System.Text;

namespace VotingCoreData.Models
{
    public class BodyMember
    {
        public int Id { get; set; }
        public int BodyId { get; set; }
        public int DeputyId { get; set; }

        public Body Body { get; set; }
        public Deputy Deputy { get; set; }
    }
}

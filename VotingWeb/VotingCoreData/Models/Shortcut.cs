using System;
using System.Collections.Generic;
using System.Text;

namespace VotingCoreData.Models
{
    public class Shortcut
    {
        public string Hash { get; set; }
        public int TopicId { get; set; }

        public Topic Topic { get; set; }
    }
}

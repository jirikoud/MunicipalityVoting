using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingCommon.Enumerations;

namespace VotingCommon.Models
{
    public class DeputyModel
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string TitlePre { get; set; }
        public string TitlePost { get; set; }
        public string Party { get; set; }
        public VoteEnum Vote { get; set; }
    }
}

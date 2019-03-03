using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingCommon.Enumerations;

namespace VotingImporter.BitEST
{
    class VoteDecoder
    {
        public static VoteEnum? Decode(string vote)
        {
            switch (vote)
            {
                case "AYE":
                    return VoteEnum.Yes;
                case "NO":
                    return VoteEnum.No;
                case "ABSTAINED":
                    return VoteEnum.Abstain;
                case "MISSING":
                    return VoteEnum.Missing;
                case "NOT_VOTING":
                    return VoteEnum.NotVoting;
            }
            return null;
        }
    }
}

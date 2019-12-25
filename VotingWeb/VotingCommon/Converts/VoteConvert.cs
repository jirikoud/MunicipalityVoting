using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingCommon.Enumerations;
using VotingCommon.Properties;

namespace VotingCommon.Converts
{
    public class VoteConvert
    {
        private static readonly List<Tuple<int, string>> voteList = new List<Tuple<int, string>>() {
            new Tuple<int, string>((int)VoteEnum.Yes, Resources.VOTE_YES),
            new Tuple<int, string>((int)VoteEnum.No, Resources.VOTE_NO),
            new Tuple<int, string>((int)VoteEnum.Abstain, Resources.VOTE_ABSTAIN),
            new Tuple<int, string>((int)VoteEnum.Missing, Resources.VOTE_MISSING),
            new Tuple<int, string>((int)VoteEnum.NotVoting, Resources.VOTE_NOT_VOTING),
        };

        public static List<Tuple<int, string>> GetVoteList()
        {
            return voteList;
        }

        public static string Convert(VoteEnum vote)
        {
            switch (vote)
            {
                case VoteEnum.Yes:
                    return Resources.VOTE_YES;
                case VoteEnum.No:
                    return Resources.VOTE_NO;
                case VoteEnum.Abstain:
                    return Resources.VOTE_ABSTAIN;
                case VoteEnum.Missing:
                    return Resources.VOTE_MISSING;
                case VoteEnum.NotVoting:
                    return Resources.VOTE_NOT_VOTING;
            }
            return null;
        }

        public static string GetItemClass(VoteEnum vote)
        {
            switch (vote)
            {
                case VoteEnum.Yes:
                    return "list-group-item-success";
                case VoteEnum.No:
                    return "list-group-item-danger";
                case VoteEnum.Abstain:
                    return "list-group-item-warning";
                case VoteEnum.Missing:
                    return null;
                case VoteEnum.NotVoting:
                    return "list-group-item-info";
            }
            return null;
        }
    }
}

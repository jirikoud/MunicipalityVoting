using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingCommon.Enumerations;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Infrastructure
{
    public class VoteConvert
    {
        private static readonly List<Tuple<int, string>> voteList = new List<Tuple<int, string>>() {
            new Tuple<int, string>((int)VoteEnum.Yes, CommonRes.VOTE_YES),
            new Tuple<int, string>((int)VoteEnum.No, CommonRes.VOTE_NO),
            new Tuple<int, string>((int)VoteEnum.Abstain, CommonRes.VOTE_ABSTAIN),
            new Tuple<int, string>((int)VoteEnum.Missing, CommonRes.VOTE_MISSING),
            new Tuple<int, string>((int)VoteEnum.NotVoting, CommonRes.VOTE_NOT_VOTING),
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
                    return CommonRes.VOTE_YES;
                case VoteEnum.No:
                    return CommonRes.VOTE_NO;
                case VoteEnum.Abstain:
                    return CommonRes.VOTE_ABSTAIN;
                case VoteEnum.Missing:
                    return CommonRes.VOTE_MISSING;
                case VoteEnum.NotVoting:
                    return CommonRes.VOTE_NOT_VOTING;
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
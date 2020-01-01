using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCommon.Enumerations;
using VotingCoreData;
using VotingCoreData.Models;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Models.Topic;

namespace VotingCoreWeb
{
    public class TopicModel : PageModel
    {
        private readonly ILogger<TopicModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        private const int NO_PARTY = -1;

        public int MunicipalityId { get; set; }
        public string MunicipalityName { get; set; }
        public int BodyId { get; set; }
        public string BodyName { get; set; }
        public int SessionId { get; set; }
        public string SessionName { get; set; }
        public int TopicId { get; set; }
        public int TopicOrder { get; set; }
        public string TopicText { get; set; }
        public string TopicName { get; set; }
        public string TopicShortname { get; set; }
        public string TopicComment { get; set; }

        public List<VoteOptionModel> VoteOptionList { get; set; }

        public List<PartyModel> PartyList { get; set; }

        public TopicModel(ILogger<TopicModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var topic = await _dbContext.GetTopicByIdAsync(id);
                var votingList = await _dbContext.GetVotingListAsync(id);

                this.MunicipalityId = topic.Session.Body.MunicipalityId;
                this.MunicipalityName = topic.Session.Body.Municipality.Name;
                this.BodyId = topic.Session.Body.Id;
                this.BodyName = topic.Session.Body.Name;
                this.SessionId = topic.Session.Id;
                this.SessionName = topic.Session.Name;
                this.TopicId = topic.Id;
                this.TopicOrder = topic.Order;
                this.TopicName = topic.Name;
                this.TopicText = topic.Text;
                this.TopicShortname = _contextUtils.ShortenString(topic.Name, 40);
                this.TopicComment = topic.Comment;

                var voteOptionDictionary = new Dictionary<VoteEnum, List<Voting>>();
                var partyDictionary = new Dictionary<int, List<Voting>>();
                foreach (var voting in votingList)
                {
                    var vote = (VoteEnum)voting.Vote;
                    if (!voteOptionDictionary.ContainsKey(vote))
                    {
                        voteOptionDictionary.Add(vote, new List<Voting>());
                    }
                    voteOptionDictionary[vote].Add(voting);

                    var partyId = voting.PartyId ?? NO_PARTY;
                    if (!partyDictionary.ContainsKey(partyId))
                    {
                        partyDictionary.Add(partyId, new List<Voting>());
                    }
                    partyDictionary[partyId].Add(voting);
                }

                this.VoteOptionList = new List<VoteOptionModel>();
                foreach (var keyValue in voteOptionDictionary)
                {
                    this.VoteOptionList.Add(new VoteOptionModel(keyValue.Key, keyValue.Value));
                }
                this.VoteOptionList.Sort((item1, item2) => item1.Vote.CompareTo(item2.Vote));

                this.PartyList = new List<PartyModel>();
                foreach (var partyVoteList in partyDictionary.Values)
                {
                    this.PartyList.Add(new PartyModel(partyVoteList));
                }
                this.PartyList.Sort((item1, item2) => item1.Name.CompareTo(item2.Name));

                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Topic({id}) failed");
                return RedirectToPage("Index");
            }
        }
    }
}
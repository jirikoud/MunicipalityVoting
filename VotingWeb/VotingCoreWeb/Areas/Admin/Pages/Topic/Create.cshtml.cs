using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using VotingCommon.Converts;
using VotingCommon.Enumerations;
using VotingCoreData;
using VotingCoreWeb.Areas.Admin.Models;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.Topic
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        [Required]
        public VotingCoreData.Models.Topic Item { get; set; }

        [BindProperty]
        public List<VotingItem> VotingList { get; set; }

        public SelectList VoteList { get; set; }

        public AlertModel Alert { get; set; }

        public CreateModel(ILogger<CreateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        private async Task PrepareListAsync(int bodyId, int sessionId)
        {
            var memberList = await _dbContext.LoadBodyMembersAsync(bodyId);
            var presentList = await _dbContext.LoadSessionMembersAsync(sessionId);
            this.VotingList = memberList.ConvertAll(item => new VotingItem(item.Deputy, presentList.Any(present => present.DeputyId == item.DeputyId) ? (int)VoteEnum.Yes : (int)VoteEnum.Missing));
            var voteList = VoteConvert.GetVoteList();
            this.VoteList = new SelectList(voteList.ConvertAll(item => new SelectListItem(item.Item2, item.Item1.ToString())), "Value", "Text");
        }

        public async Task<IActionResult> OnGetAsync(int sessionId)
        {
            try
            {
                var session = await _dbContext.FindSessionByIdAsync(sessionId);
                if (session == null)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(session.Body.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                var count = await _dbContext.GetSessionTopicCountAsync(session.Id);
                this.Item = new VotingCoreData.Models.Topic()
                {
                    SessionId = sessionId,
                    Order = (count ?? 0) + 1,
                };
                await PrepareListAsync(session.BodyId, session.Id);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Topic/Index", new { area = "Admin", id = sessionId });
            }
        }

        public async Task<IActionResult> OnPostAsync(string handler)
        {
            try
            {
                var session = await _dbContext.FindSessionByIdAsync(this.Item.SessionId);
                if (session == null)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(session.Body.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                if (ModelState.IsValid)
                {
                    var votings = this.VotingList.ConvertAll(item => new VotingCoreData.Models.Voting() { DeputyId = item.DeputyId, PartyId = item.PartyId, Vote = item.Vote });
                    var itemId = await _dbContext.UpdateTopicAsync(null, this.Item, votings);
                    if (itemId.HasValue)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                        if (handler == "Voting")
                        {
                            return RedirectToPage("/Voting/Index", new { area = "Admin", id = itemId.Value });
                        }
                        return RedirectToPage("/Topic/Index", new { area = "Admin", id = this.Item.SessionId });
                    }
                    else
                    {
                        Alert = new AlertModel(AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_CREATE);
                    }
                }
                await PrepareListAsync(session.BodyId, session.Id);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Topic/Index", new { area = "Admin" });
            }
        }
    }
}

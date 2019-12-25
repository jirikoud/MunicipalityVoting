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
using VotingCoreData;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.Voting
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        [Required]
        public VotingCoreData.Models.Voting Item { get; set; }

        public SelectList DeputyList { get; set; }
        public SelectList PartyList { get; set; }
        public SelectList VoteList { get; set; }

        public AlertModel Alert { get; set; }

        public CreateModel(ILogger<CreateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        private async Task PrepareSelectListAsync(int municipalityId)
        {
            var deputyList = await _dbContext.LoadDeputiesAsync(municipalityId);
            this.DeputyList = new SelectList(deputyList.ConvertAll(item => new SelectListItem(item.GetFullName(), item.Id.ToString())), "Value", "Text");
            var partyList = await _dbContext.LoadPartiesAsync(municipalityId);
            this.PartyList = new SelectList(partyList.ConvertAll(item => new SelectListItem(item.Name, item.Id.ToString())), "Value", "Text");
            var voteList = VoteConvert.GetVoteList();
            this.VoteList = new SelectList(voteList.ConvertAll(item => new SelectListItem(item.Item2, item.Item1.ToString())), "Value", "Text");
        }

        public async Task<IActionResult> OnGetAsync(int topicId)
        {
            try
            {
                var topic = await _dbContext.FindTopicByIdAsync(topicId);
                if (topic == null)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(topic.Session.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                this.Item = new VotingCoreData.Models.Voting()
                {
                    TopicId = topicId,
                };
                await PrepareSelectListAsync(topic.Session.MunicipalityId);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Voting/Index", new { area = "Admin", id = topicId });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var topic = await _dbContext.FindTopicByIdAsync(this.Item.TopicId);
                if (topic == null)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(topic.Session.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                if (ModelState.IsValid)
                {
                    var itemId = await _dbContext.UpdateVotingAsync(null, this.Item);
                    if (itemId.HasValue)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                        return RedirectToPage("/Voting/Index", new { area = "Admin", id = this.Item.TopicId });
                    }
                    else
                    {
                        Alert = new AlertModel(AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_CREATE);
                    }
                }
                await PrepareSelectListAsync(checkId.Value);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Voting/Index", new { area = "Admin" });
            }
        }
    }
}

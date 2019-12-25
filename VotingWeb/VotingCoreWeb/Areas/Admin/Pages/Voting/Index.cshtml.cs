using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCoreData;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.Voting
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        public int SessionId { get; set; }
        public int TopicId { get; set; }
        public List<VotingCoreData.Models.Voting> ItemList { get; set; }

        public IndexModel(ILogger<IndexModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var topic = await _dbContext.FindTopicByIdAsync(id);
                if (topic == null)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                int? municipalityId = await _contextUtils.CheckMunicipalityRightsAsync(topic.Session.MunicipalityId, User, _dbContext, TempData);
                if (!municipalityId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                this.TopicId = topic.Id;
                this.SessionId = topic.SessionId;
                this.ItemList = await _dbContext.LoadVotingsAsync(topic.Id);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Index failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Index", new { area = "" });
            }
        }
    }
}

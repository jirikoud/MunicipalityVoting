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

namespace VotingCoreWeb.Areas.Admin.Pages.Topic
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        public int SessionId { get; set; }
        public int BodyId { get; set; }
        public List<VotingCoreData.Models.Topic> ItemList { get; set; }

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
                var session = await _dbContext.FindSessionByIdAsync(id);
                if (session == null)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                int? municipalityId = await _contextUtils.CheckMunicipalityRightsAsync(session.Body.MunicipalityId, User, _dbContext, TempData);
                if (!municipalityId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                this.SessionId = session.Id;
                this.BodyId = session.BodyId;
                this.ItemList = await _dbContext.LoadTopicsAsync(session.Id);
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

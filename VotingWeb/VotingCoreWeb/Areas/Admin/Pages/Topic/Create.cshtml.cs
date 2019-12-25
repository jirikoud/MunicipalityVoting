using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        [Required]
        public VotingCoreData.Models.Topic Item { get; set; }

        public AlertModel Alert { get; set; }

        public CreateModel(ILogger<CreateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
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
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(session.MunicipalityId, User, _dbContext, TempData);
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
                if (ModelState.IsValid)
                {
                    var session = await _dbContext.FindSessionByIdAsync(this.Item.SessionId);
                    if (session == null)
                    {
                        return RedirectToPage("/Index", new { area = "" });
                    }
                    var checkId = await _contextUtils.CheckMunicipalityRightsAsync(session.MunicipalityId, User, _dbContext, TempData);
                    if (!checkId.HasValue)
                    {
                        return RedirectToPage("/Index", new { area = "" });
                    }
                    var itemId = await _dbContext.UpdateTopicAsync(null, this.Item);
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

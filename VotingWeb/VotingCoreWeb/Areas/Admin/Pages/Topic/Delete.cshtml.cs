using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCoreData;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.Topic
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        public VotingCoreData.Models.Topic Item { get; set; }

        public AlertModel Alert { get; set; }

        public DeleteModel(ILogger<DeleteModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
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
                if (topic == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                    return RedirectToPage("/Topic/Index", new { area = "Admin" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(topic.Session.Body.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Topic/Index", new { area = "Admin" });
                }
                this.Item = topic;
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Delete failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Topic/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (this.Item != null)
                {
                    var topic = await _dbContext.GetTopicByIdAsync(this.Item.Id);
                    if (topic == null)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                        return RedirectToPage("/Topic/Index", new { area = "Admin" });
                    }
                    var checkId = await _contextUtils.CheckMunicipalityRightsAsync(topic.Session.Body.MunicipalityId, User, _dbContext, TempData);
                    if (!checkId.HasValue)
                    {
                        return RedirectToPage("/Topic/Index", new { area = "Admin" });
                    }
                    var isSuccess = await _dbContext.DeleteTopicAsync(topic.Id);
                    if (isSuccess)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_DELETE);
                        return RedirectToPage("/Topic/Index", new { area = "Admin", id = topic.SessionId });
                    }
                    else
                    {
                        Alert = new AlertModel(AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_DELETE);
                    }
                }
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Delete() failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Topic/Index", new { area = "Admin" });
            }
        }

    }
}

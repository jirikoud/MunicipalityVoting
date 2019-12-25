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

namespace VotingCoreWeb.Areas.Admin.Pages.Session
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        public VotingCoreData.Models.Session Item { get; set; }

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
                var session = await _dbContext.FindSessionByIdAsync(id);
                if (session == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                    return RedirectToPage("/Session/Index", new { area = "Admin" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(session.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Session/Index", new { area = "Admin" });
                }
                this.Item = session;
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Delete failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Session/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (this.Item != null)
                {
                    var session = await _dbContext.FindSessionByIdAsync(this.Item.Id);
                    if (session == null)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                        return RedirectToPage("/Session/Index", new { area = "Admin" });
                    }
                    var checkId = await _contextUtils.CheckMunicipalityRightsAsync(session.MunicipalityId, User, _dbContext, TempData);
                    if (!checkId.HasValue)
                    {
                        return RedirectToPage("/Session/Index", new { area = "Admin" });
                    }
                    var isSuccess = await _dbContext.DeleteSessionAsync(session.Id);
                    if (isSuccess)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_DELETE);
                        return RedirectToPage("/Session/Index", new { area = "Admin", id = session.MunicipalityId });
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
                return RedirectToPage("/Session/Index", new { area = "Admin" });
            }
        }

    }
}

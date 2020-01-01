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

namespace VotingCoreWeb.Areas.Admin.Pages.Party
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        public VotingCoreData.Models.Party Item { get; set; }

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
                var party = await _dbContext.GetPartyByIdAsync(id);
                if (party == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                    return RedirectToPage("/Party/Index", new { area = "Admin" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(party.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Party/Index", new { area = "Admin" });
                }
                this.Item = party;
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Delete failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Party/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (this.Item != null)
                {
                    var party = await _dbContext.GetPartyByIdAsync(this.Item.Id);
                    if (party == null)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                        return RedirectToPage("/Party/Index", new { area = "Admin" });
                    }
                    var checkId = await _contextUtils.CheckMunicipalityRightsAsync(party.MunicipalityId, User, _dbContext, TempData);
                    if (!checkId.HasValue)
                    {
                        return RedirectToPage("/Party/Index", new { area = "Admin" });
                    }
                    var isSuccess = await _dbContext.DeletePartyAsync(party.Id);
                    if (isSuccess)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_DELETE);
                        return RedirectToPage("/Party/Index", new { area = "Admin", id = party.MunicipalityId });
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
                return RedirectToPage("/Party/Index", new { area = "Admin" });
            }
        }

    }
}

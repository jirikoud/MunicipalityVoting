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

namespace VotingCoreWeb.Areas.Admin.Pages.Party
{
    [Authorize]
    public class UpdateModel : PageModel
    {
        private readonly ILogger<UpdateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        [Required]
        public VotingCoreData.Models.Party Item { get; set; }

        public AlertModel Alert { get; set; }

        public UpdateModel(ILogger<UpdateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var party = await _dbContext.FindPartyByIdAsync(id);
                if (party == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(party.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                this.Item = party;
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Update failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Party/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostAsync(string handler)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var party = await _dbContext.FindPartyByIdAsync(this.Item.Id);
                    if (party == null)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                        return RedirectToPage("/Index", new { area = "" });
                    }
                    var checkId = await _contextUtils.CheckMunicipalityRightsAsync(party.MunicipalityId, User, _dbContext, TempData);
                    if (!checkId.HasValue)
                    {
                        return RedirectToPage("/Index", new { area = "" });
                    }
                    var itemId = await _dbContext.UpdatePartyAsync(party.Id, this.Item);
                    if (itemId.HasValue)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
                        return RedirectToPage("/Party/Index", new { area = "Admin", id = party.MunicipalityId });
                    }
                    else
                    {
                        Alert = new AlertModel(AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_UPDATE);
                    }
                }
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Update failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Party/Index", new { area = "Admin" });
            }
        }
    }
}

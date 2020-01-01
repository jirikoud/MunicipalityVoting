using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCommon;
using VotingCoreData;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.Municipality
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class UpdateModel : PageModel
    {
        private readonly ILogger<UpdateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        [Required]
        public VotingCoreData.Models.Municipality Item { get; set; }

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
                this.Item = await _dbContext.GetMunicipalityByIdAsync(id);
                if (this.Item == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                    return RedirectToPage("/Municipality/Index", new { area = "Admin" });
                }
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Update failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Municipality/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostAsync(string handler)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var itemId = await _dbContext.UpdateMunicipalityAsync(this.Item.Id, this.Item);
                    if (itemId.HasValue)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
                        if (handler == "Bodies")
                        {
                            return RedirectToPage("/Body/Index", new { area = "Admin", id = itemId.Value });
                        }
                        return RedirectToPage("/Municipality/Index", new { area = "Admin" });
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
                return RedirectToPage("/Municipality/Index", new { area = "Admin" });
            }
        }
    }
}

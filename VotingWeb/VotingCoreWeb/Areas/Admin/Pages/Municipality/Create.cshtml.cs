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
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        [Required]
        public VotingCoreData.Models.Municipality Item { get; set; }

        public AlertModel Alert { get; set; }

        public CreateModel(ILogger<CreateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
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
                    var itemId = await _dbContext.UpdateMunicipalityAsync(null, this.Item);
                    if (itemId.HasValue)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                        if (handler == "Sessions")
                        {
                            return RedirectToPage("/Session/Index", new { area = "Admin", id = itemId.Value });
                        }
                        return RedirectToPage("Index");
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
                return RedirectToPage("/Municipality/Index", new { area = "Admin" });
            }
        }
    }
}

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

namespace VotingCoreWeb.Areas.Admin.Pages.Municipality
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly VotingDbContext _dbContext;

        [BindProperty]
        [Display(Name = "DETAIL_NAME", ResourceType = typeof(MunicipalityAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string Name { get; set; }

        [BindProperty]
        [Display(Name = "DETAIL_DESCRIPTION", ResourceType = typeof(MunicipalityAdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string Description { get; set; }

        public AlertModel Alert { get; set; }

        public CreateModel(ILogger<CreateModel> logger, VotingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        private async Task<int?> TryCreateAsync()
        {
            var municipality = new VotingCoreData.Models.Municipality()
            {
                Name = this.Name,
                Description = this.Description,
            };
            return await _dbContext.CreateMunicipalityAsync(municipality);
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
                ContextUtils.Instance.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Municipality/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var itemId = await TryCreateAsync();
                    if (itemId.HasValue)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
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
                ContextUtils.Instance.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Municipality/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostSessionsAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var itemId = await TryCreateAsync();
                    if (itemId.HasValue)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                        return RedirectToPage("/Session/Index", new { id = itemId.Value });
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
                ContextUtils.Instance.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Municipality/Index", new { area = "Admin" });
            }
        }
    }
}

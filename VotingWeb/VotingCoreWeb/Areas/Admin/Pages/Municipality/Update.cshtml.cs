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
    public class UpdateModel : PageModel
    {
        private readonly ILogger<UpdateModel> _logger;
        private readonly VotingDbContext _dbContext;

        [BindProperty]
        public int Id { get; set; }

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

        public UpdateModel(ILogger<UpdateModel> logger, VotingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        private async Task<bool> TryUpdateAsync()
        {
            var municipality = await _dbContext.FindMunicipalityByIdAsync(this.Id);
            municipality.Name = this.Name;
            municipality.Description = this.Description;
            return await _dbContext.UpdateAsync();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var item = await _dbContext.FindMunicipalityByIdAsync(id);
                this.Id = id;
                this.Name = item.Name;
                this.Description = item.Description;
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
                    var isSuccess = await TryUpdateAsync();
                    if (isSuccess)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
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
                    var isSuccess = await TryUpdateAsync();
                    if (isSuccess)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
                        return RedirectToPage("/Session/Index", new { id = this.Id });
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
                _logger.LogError(exception, "Update failed");
                ContextUtils.Instance.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Municipality/Index", new { area = "Admin" });
            }
        }
    }
}

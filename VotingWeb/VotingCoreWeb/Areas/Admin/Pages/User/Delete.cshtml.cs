using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCoreData;
using VotingCoreWeb.Areas.Admin.Models;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.User
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> _logger;
        private readonly ContextUtils _contextUtils;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public UserModel Item { get; set; }

        public AlertModel Alert { get; set; }

        public DeleteModel(ILogger<DeleteModel> logger, ContextUtils contextUtils, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _contextUtils = contextUtils;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                    return RedirectToPage("/User/Index", new { area = "Admin" });
                }
                this.Item = new UserModel(user);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Delete failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/User/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (this.Item != null)
                {
                    var user = await _userManager.FindByIdAsync(this.Item.Id);
                    if (user == null)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                        return RedirectToPage("/User/Index", new { area = "Admin" });
                    }
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_DELETE);
                        return RedirectToPage("/User/Index", new { area = "Admin" });
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
                return RedirectToPage("/User/Index", new { area = "Admin" });
            }
        }

    }
}

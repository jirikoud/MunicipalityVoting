using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
            [DataType(DataType.Password)]
            [Display(Name = "CHANGE_PASSWORD_OLD", ResourceType = typeof(AccountRes))]
            public string OldPassword { get; set; }

            [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
            [StringLength(100, MinimumLength = 6, ErrorMessageResourceName = "VALIDATION_LENGTH_FULL", ErrorMessageResourceType = typeof(AdminRes))]
            [DataType(DataType.Password)]
            [Display(Name = "CHANGE_PASSWORD_NEW", ResourceType = typeof(AccountRes))]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "CHANGE_PASSWORD_CONFIRM", ResourceType = typeof(AccountRes))]
            [Compare("NewPassword", ErrorMessageResourceName = "VALIDATION_PASSWORD_COMPARE", ErrorMessageResourceType = typeof(AdminRes))]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = AccountRes.MESSAGE_PASSWORD_CHANGED;

            return RedirectToPage();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VotingCoreData;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ApiKeyModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly VotingDbContext _dbContext;

        public ApiKeyModel(UserManager<IdentityUser> userManager, VotingDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public string ApiKey { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        private async Task LoadKeyAsync(string userId)
        {
            this.ApiKey = await _dbContext.GetApiKeyAsync(userId);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await LoadKeyAsync(user.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadKeyAsync(user.Id);
                return Page();
            }

            var isSuccess = await _dbContext.CreateApiKeyAsync(user.Id);
            if (isSuccess)
            {
                StatusMessage = StatusMessageModel.Create(AccountRes.MESSAGE_API_KEY_GENERATED);
            }
            else
            {
                StatusMessage = StatusMessageModel.Create(AccountRes.MESSAGE_API_KEY_FAILED, true);
            }
            return RedirectToPage();
        }

    }
}

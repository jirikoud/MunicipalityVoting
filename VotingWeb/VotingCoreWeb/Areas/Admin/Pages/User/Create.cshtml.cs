using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using VotingCommon;
using VotingCommon.Converts;
using VotingCoreData;
using VotingCoreWeb.Areas.Admin.Models;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.User
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        [Required]
        public UserModel Item { get; set; }

        public SelectList RoleList { get; set; }
        public SelectList MunicipalityList { get; set; }

        public AlertModel Alert { get; set; }

        public CreateModel(ILogger<CreateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
            _userManager = userManager;
        }

        public async Task PrepareSelectListAsync()
        {
            this.RoleList = new SelectList(RoleConvert.GetRoleList(), "Item1", "Item2");
            var municipalityList = await _dbContext.GetMunicipalityListAsync();
            this.MunicipalityList = new SelectList(municipalityList.ConvertAll(item => new SelectListItem(item.Name, item.Id.ToString())), "Value", "Text");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await PrepareSelectListAsync();
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Municipality/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new IdentityUser
                    {
                        UserName = this.Item.UserName,
                        Email = this.Item.UserName,
                        EmailConfirmed = true
                    };
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrWhiteSpace(this.Item.Password))
                        {
                            await _userManager.AddPasswordAsync(user, this.Item.Password);
                        }
                        await _userManager.AddToRoleAsync(user, this.Item.Role);
                        await _userManager.AddClaimAsync(user, new Claim(Constants.CLAIM_MUNICIPALITY, this.Item.MunicipalityId.ToString()));
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                        return RedirectToPage("Index");
                    }
                    else
                    {
                        Alert = new AlertModel(AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_CREATE);
                    }
                }
                await PrepareSelectListAsync();
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/User/Index", new { area = "Admin" });
            }
        }
    }
}

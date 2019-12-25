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
    public class UpdateModel : PageModel
    {
        private readonly ILogger<UpdateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        [Required]
        public UserModel Item { get; set; }

        public SelectList RoleList { get; set; }
        public SelectList MunicipalityList { get; set; }

        public AlertModel Alert { get; set; }

        public UpdateModel(ILogger<UpdateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
            _userManager = userManager;
        }

        public async Task PrepareSelectListAsync()
        {
            this.RoleList = new SelectList(RoleConvert.GetRoleList(), "Item1", "Item2");
            var municipalityList = await _dbContext.LoadMunicipalitiesAsync();
            this.MunicipalityList = new SelectList(municipalityList.ConvertAll(item => new SelectListItem(item.Name, item.Id.ToString())), "Value", "Text");
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
                var roles = await _userManager.GetRolesAsync(user);
                this.Item.Role = roles.FirstOrDefault(item => RoleConvert.Decode(item) != null);
                var claims = await _userManager.GetClaimsAsync(user);
                this.Item.MunicipalityId = claims.Where(item => item.Type == Constants.CLAIM_MUNICIPALITY).Select(item => int.Parse(item.Value)).FirstOrDefault();
                await PrepareSelectListAsync();
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Update failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/User/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(this.Item.Id);
                    var currentRoleList = await _userManager.GetRolesAsync(user);
                    bool hasRole = false;
                    foreach (var role in currentRoleList)
                    {
                        if (role != this.Item.Role)
                        {
                            await _userManager.RemoveFromRoleAsync(user, role);
                        }
                        else
                        {
                            hasRole = true;
                        }
                    }
                    if (!hasRole)
                    {
                        await _userManager.AddToRoleAsync(user, this.Item.Role);
                    }
                    var claimsToRemove = await _userManager.GetClaimsAsync(user);
                    foreach (var claim in claimsToRemove)
                    {
                        await _userManager.RemoveClaimAsync(user, claim);
                    }
                    await _userManager.AddClaimAsync(user, new Claim(Constants.CLAIM_MUNICIPALITY, this.Item.MunicipalityId.ToString()));
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrWhiteSpace(this.Item.Password))
                        {
                            var hasPassword = await _userManager.HasPasswordAsync(user);
                            if (hasPassword)
                            {
                                await _userManager.RemovePasswordAsync(user);
                            }
                            await _userManager.AddPasswordAsync(user, this.Item.Password);
                        }
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
                        return RedirectToPage("/User/Index", new { area = "Admin" });
                    }
                    else
                    {
                        Alert = new AlertModel(AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_UPDATE);
                    }
                }
                await PrepareSelectListAsync();
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Update failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/User/Index", new { area = "Admin" });
            }
        }
    }
}

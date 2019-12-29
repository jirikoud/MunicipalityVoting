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
using VotingCoreData.Models;
using VotingCoreWeb.Areas.Admin.Models;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.Body
{
    [Authorize]
    public class UpdateModel : PageModel
    {
        private readonly ILogger<UpdateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        [Required]
        public VotingCoreData.Models.Body Item { get; set; }

        [BindProperty]
        public List<MemberItem> MemberList { get; set; }

        public AlertModel Alert { get; set; }

        public UpdateModel(ILogger<UpdateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        private async Task PrepareListAsync(VotingCoreData.Models.Body body)
        {
            var deputyList = await _dbContext.LoadDeputiesAsync(body.MunicipalityId);
            this.MemberList = deputyList.ConvertAll(item => new MemberItem(item, body.BodyMembers.Any(member => member.DeputyId == item.Id)));
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var body = await _dbContext.FindBodyByIdAsync(id);
                if (body == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(body.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                this.Item = body;
                await PrepareListAsync(body);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Update failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Body/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostAsync(string handler)
        {
            try
            {
                var body = await _dbContext.FindBodyByIdAsync(this.Item.Id);
                if (body == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(body.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                if (ModelState.IsValid)
                {
                    var members = this.MemberList.Where(item => item.IsChecked).Select(item => new BodyMember() { DeputyId = item.DeputyId }).ToList();
                    var itemId = await _dbContext.UpdateBodyAsync(body.Id, this.Item, members);
                    if (itemId.HasValue)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
                        if (handler == "Sessions")
                        {
                            return RedirectToPage("/Session/Index", new { area = "Admin", id = itemId.Value });
                        }
                        return RedirectToPage("/Body/Index", new { area = "Admin", id = body.MunicipalityId });
                    }
                    else
                    {
                        Alert = new AlertModel(AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_UPDATE);
                    }
                }
                await PrepareListAsync(body);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Update failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Body/Index", new { area = "Admin" });
            }
        }
    }
}

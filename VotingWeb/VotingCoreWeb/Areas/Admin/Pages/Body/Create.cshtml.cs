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
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        [Required]
        public VotingCoreData.Models.Body Item { get; set; }

        [BindProperty]
        public List<MemberItem> MemberList { get; set; }

        public AlertModel Alert { get; set; }

        public CreateModel(ILogger<CreateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        private async Task PrepareListAsync(int municipalityId)
        {
            var deputyList = await _dbContext.LoadDeputiesAsync(municipalityId);
            this.MemberList = deputyList.ConvertAll(item => new MemberItem(item, false));
        }

        public async Task<IActionResult> OnGetAsync(int municipalityId)
        {
            try
            {
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(municipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                this.Item = new VotingCoreData.Models.Body()
                {
                    MunicipalityId = municipalityId,
                };
                await PrepareListAsync(municipalityId);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Body/Index", new { area = "Admin", id = municipalityId });
            }
        }

        public async Task<IActionResult> OnPostAsync(string handler)
        {
            try
            {
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(this.Item.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                if (ModelState.IsValid)
                {
                    var members = this.MemberList.Where(item => item.IsChecked).Select(item => new BodyMember() { DeputyId = item.DeputyId }).ToList();
                    var itemId = await _dbContext.UpdateBodyAsync(null, this.Item, members);
                    if (itemId.HasValue)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                        if (handler == "Sessions")
                        {
                            return RedirectToPage("/Session/Index", new { area = "Admin", id = itemId.Value });
                        }
                        return RedirectToPage("/Body/Index", new { area = "Admin", id = this.Item.MunicipalityId });
                    }
                    else
                    {
                        Alert = new AlertModel(AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_CREATE);
                    }
                }
                await PrepareListAsync(this.Item.MunicipalityId);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Body/Index", new { area = "Admin" });
            }
        }
    }
}

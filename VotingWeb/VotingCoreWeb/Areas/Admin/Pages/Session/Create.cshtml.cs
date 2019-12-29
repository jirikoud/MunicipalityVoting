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

namespace VotingCoreWeb.Areas.Admin.Pages.Session
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        [Required]
        public VotingCoreData.Models.Session Item { get; set; }

        [BindProperty]
        public List<MemberItem> MemberList { get; set; }

        public AlertModel Alert { get; set; }

        public CreateModel(ILogger<CreateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        private async Task PrepareListAsync(int bodyId)
        {
            var memberList = await _dbContext.LoadBodyMembersAsync(bodyId);
            this.MemberList = memberList.ConvertAll(item => new MemberItem(item.Deputy, true));
        }

        public async Task<IActionResult> OnGetAsync(int bodyId)
        {
            try
            {
                var body = await _dbContext.FindBodyByIdAsync(bodyId);
                if (body == null)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(body.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                this.Item = new VotingCoreData.Models.Session()
                {
                    BodyId = bodyId,
                };
                await PrepareListAsync(body.Id);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Session/Index", new { area = "Admin", id = bodyId });
            }
        }

        public async Task<IActionResult> OnPostAsync(string handler)
        {
            try
            {
                var body = await _dbContext.FindBodyByIdAsync(this.Item.BodyId);
                if (body == null)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(body.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                if (ModelState.IsValid)
                {
                    var members = this.MemberList.Where(item => item.IsChecked).Select(item => new SessionMember() { DeputyId = item.DeputyId }).ToList();
                    var itemId = await _dbContext.UpdateSessionAsync(null, this.Item, members);
                    if (itemId.HasValue)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                        if (handler == "Topics")
                        {
                            return RedirectToPage("/Topic/Index", new { area = "Admin", id = itemId.Value });
                        }
                        return RedirectToPage("/Session/Index", new { area = "Admin", id = this.Item.BodyId });
                    }
                    else
                    {
                        Alert = new AlertModel(AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_CREATE);
                    }
                }
                await PrepareListAsync(body.Id);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Session/Index", new { area = "Admin" });
            }
        }
    }
}

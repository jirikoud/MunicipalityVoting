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
    public class UpdateModel : PageModel
    {
        private readonly ILogger<UpdateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        [Required]
        public VotingCoreData.Models.Session Item { get; set; }

        [BindProperty]
        public List<MemberItem> MemberList { get; set; }

        public AlertModel Alert { get; set; }

        public UpdateModel(ILogger<UpdateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        private async Task PrepareListAsync(VotingCoreData.Models.Session session)
        {
            var deputyList = await _dbContext.GetBodyMemberListAsync(session.BodyId);
            this.MemberList = deputyList.ConvertAll(item => new MemberItem(item.Deputy, session.SessionMembers.Any(member => member.DeputyId == item.DeputyId)));
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var session = await _dbContext.GetSessionByIdAsync(id);
                if (session == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(session.Body.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                this.Item = session;
                await PrepareListAsync(session);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Update failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Index", new { area = "" });
            }
        }

        public async Task<IActionResult> OnPostAsync(string handler)
        {
            try
            {
                var session = await _dbContext.GetSessionByIdAsync(this.Item.Id);
                if (session == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(session.Body.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                if (ModelState.IsValid)
                {
                    var members = this.MemberList.Where(item => item.IsChecked).Select(item => new SessionMember() { DeputyId = item.DeputyId }).ToList();
                    var itemId = await _dbContext.UpdateSessionAsync(session.Id, this.Item, members);
                    if (itemId.HasValue)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
                        if (handler == "Topics")
                        {
                            return RedirectToPage("/Topic/Index", new { area = "Admin", id = itemId.Value });
                        }
                        return RedirectToPage("/Session/Index", new { area = "Admin", id = session.BodyId });
                    }
                    else
                    {
                        Alert = new AlertModel(AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_UPDATE);
                    }
                }
                await PrepareListAsync(session);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Update failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Session/Index", new { area = "Admin" });
            }
        }
    }
}

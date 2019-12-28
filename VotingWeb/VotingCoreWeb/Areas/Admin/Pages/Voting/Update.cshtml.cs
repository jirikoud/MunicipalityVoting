using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using VotingCommon;
using VotingCommon.Converts;
using VotingCoreData;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.Voting
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class UpdateModel : PageModel
    {
        private readonly ILogger<UpdateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        [Required]
        public VotingCoreData.Models.Voting Item { get; set; }

        public SelectList DeputyList { get; set; }
        public SelectList PartyList { get; set; }
        public SelectList VoteList { get; set; }

        public AlertModel Alert { get; set; }

        public UpdateModel(ILogger<UpdateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        private async Task PrepareSelectListAsync(int municipalityId)
        {
            var deputyList = await _dbContext.LoadDeputiesAsync(municipalityId);
            this.DeputyList = new SelectList(deputyList.ConvertAll(item => new SelectListItem(item.GetFullName(), item.Id.ToString())), "Value", "Text");
            var partyList = await _dbContext.LoadPartiesAsync(municipalityId);
            this.PartyList = new SelectList(partyList.ConvertAll(item => new SelectListItem(item.Name, item.Id.ToString())), "Value", "Text");
            var voteList = VoteConvert.GetVoteList();
            this.VoteList = new SelectList(voteList.ConvertAll(item => new SelectListItem(item.Item2, item.Item1.ToString())), "Value", "Text");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var voting = await _dbContext.FindVotingByIdAsync(id);
                if (voting == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(voting.Topic.Session.Body.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                this.Item = voting;
                await PrepareSelectListAsync(voting.Topic.Session.Body.MunicipalityId);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Update failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Voting/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var voting = await _dbContext.FindVotingByIdAsync(this.Item.Id);
                if (voting == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                    return RedirectToPage("/Index", new { area = "" });
                }
                var checkId = await _contextUtils.CheckMunicipalityRightsAsync(voting.Topic.Session.Body.MunicipalityId, User, _dbContext, TempData);
                if (!checkId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                if (ModelState.IsValid)
                {
                    var itemId = await _dbContext.UpdateVotingAsync(voting.Id, this.Item);
                    if (itemId.HasValue)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
                        return RedirectToPage("/Voting/Index", new { area = "Admin", id = voting.TopicId });
                    }
                    else
                    {
                        Alert = new AlertModel(AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_UPDATE);
                    }
                }
                await PrepareSelectListAsync(checkId.Value);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Update failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Voting/Index", new { area = "Admin" });
            }
        }
    }
}

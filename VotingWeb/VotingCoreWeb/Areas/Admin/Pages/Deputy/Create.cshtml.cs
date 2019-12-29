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
using VotingCoreData;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.Deputy
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        [BindProperty]
        [Required]
        public VotingCoreData.Models.Deputy Item { get; set; }

        public SelectList PartyList { get; set; }

        public AlertModel Alert { get; set; }

        public CreateModel(ILogger<CreateModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        private async Task PrepareSelectListAsync(int municipalityId)
        {
            var partyList = await _dbContext.LoadPartiesAsync(municipalityId);
            this.PartyList = new SelectList(partyList.ConvertAll(item => new SelectListItem(item.Name, item.Id.ToString())), "Value", "Text");
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
                this.Item = new VotingCoreData.Models.Deputy()
                {
                    MunicipalityId = municipalityId,
                };
                await PrepareSelectListAsync(municipalityId);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Deputy/Index", new { area = "Admin", id = municipalityId });
            }
        }

        public async Task<IActionResult> OnPostAsync()
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
                    var itemId = await _dbContext.UpdateDeputyAsync(null, this.Item);
                    if (itemId.HasValue)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                        return RedirectToPage("/Deputy/Index", new { area = "Admin", id = this.Item.MunicipalityId });
                    }
                    else
                    {
                        Alert = new AlertModel(AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_CREATE);
                    }
                }
                await PrepareSelectListAsync(this.Item.MunicipalityId);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Create failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Deputy/Index", new { area = "Admin" });
            }
        }
    }
}

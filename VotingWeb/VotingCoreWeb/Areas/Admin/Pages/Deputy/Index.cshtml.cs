using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCoreData;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.Deputy
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;

        public int MunicipalityId { get; set; }
        public List<VotingCoreData.Models.Deputy> ItemList { get; set; }

        public IndexModel(ILogger<IndexModel> logger, VotingDbContext dbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                int? municipalityId = await _contextUtils.CheckMunicipalityRightsAsync(id, User, _dbContext, TempData);
                if (!municipalityId.HasValue)
                {
                    return RedirectToPage("/Index", new { area = "" });
                }
                this.MunicipalityId = municipalityId.Value;
                this.ItemList = await _dbContext.LoadDeputiesAsync(municipalityId.Value);
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Index failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Index", new { area = "" });
            }
        }
    }
}

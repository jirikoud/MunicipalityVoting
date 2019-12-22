using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCoreData;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.Municipality
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly VotingDbContext _dbContext;

        public List<VotingCoreData.Models.Municipality> ItemList { get; set; }

        public IndexModel(ILogger<IndexModel> logger, VotingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                this.ItemList = await _dbContext.LoadMunicipalitiesAsync();
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Index failed");
                ContextUtils.Instance.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Index", new { area = "" });
            }
        }
    }
}

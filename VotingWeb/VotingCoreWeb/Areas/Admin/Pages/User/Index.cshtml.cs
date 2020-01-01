using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCommon;
using VotingCoreData;
using VotingCoreWeb.Data;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Areas.Admin.Pages.User
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;
        private readonly ApplicationDbContext _appDbContext;

        public List<VotingCoreData.Models.User> ItemList { get; set; }

        public List<VotingCoreData.Models.Municipality> Municipalities { get; set; }

        public IndexModel(ILogger<IndexModel> logger, VotingDbContext dbContext, ApplicationDbContext appDbContext, ContextUtils contextUtils)
        {
            _logger = logger;
            _dbContext = dbContext;
            _appDbContext = appDbContext;
            _contextUtils = contextUtils;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                this.Municipalities = await _dbContext.GetMunicipalityListAsync();
                var users = await _dbContext.GetUserListAsync();
                this.ItemList = users;
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

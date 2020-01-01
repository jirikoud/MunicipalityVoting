using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCoreData;
using VotingCoreWeb.Models.Home;

namespace VotingCoreWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly VotingDbContext _dbContext;

        public List<MunicipalityItem> MunicipalityList;

        public IndexModel(ILogger<IndexModel> logger, VotingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnGetAsync(string hash)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hash))
                {
                    var shortcut = await _dbContext.FindShortcutByHashAsync(hash);
                    if (shortcut != null)
                    {
                        return RedirectToAction("Index", "Topic", new { id = shortcut.TopicId });
                    }
                }
                this.MunicipalityList = new List<MunicipalityItem>();
                var municipalityList = await _dbContext.GetMunicipalityListAsync();
                foreach (var municipality in municipalityList)
                {
                    var bodyList = await _dbContext.GetBodyListAsync(municipality.Id);
                    this.MunicipalityList.Add(new MunicipalityItem(municipality, bodyList));
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Home failed");
            }
            return Page();
        }
    }
}

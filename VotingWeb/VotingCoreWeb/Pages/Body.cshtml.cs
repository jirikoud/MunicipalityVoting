using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCoreData;
using VotingCoreWeb.Models.Body;

namespace VotingCoreWeb
{
    public class BodyModel : PageModel
    {
        private readonly ILogger<MunicipalityModel> _logger;
        private readonly VotingDbContext _dbContext;

        public int MunicipalityId { get; set; }
        public string MunicipalityName { get; set; }
        public string BodyName { get; set; }
        public List<SessionItem> SessionList { get; set; }

        public BodyModel(ILogger<MunicipalityModel> logger, VotingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var body = await _dbContext.GetBodyByIdAsync(id);
                var sessionList = await _dbContext.GetSessionListAsync(id);
                this.MunicipalityId = body.MunicipalityId;
                this.MunicipalityName = body.Municipality.Name;
                this.BodyName = body.Name;
                this.SessionList = sessionList.ConvertAll(item => new SessionItem(item));
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Body({id}) failed");
                return RedirectToPage("Index");
            }
        }
    }
}
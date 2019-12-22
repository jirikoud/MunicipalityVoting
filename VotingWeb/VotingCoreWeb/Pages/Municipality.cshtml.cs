﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCoreData;
using VotingCoreWeb.Models.Municipality;

namespace VotingCoreWeb
{
    public class MunicipalityModel : PageModel
    {
        private readonly ILogger<MunicipalityModel> _logger;
        private readonly VotingDbContext _dbContext;

        public string MunicipalityName { get; set; }
        public List<SessionItem> SessionList { get; set; }

        public MunicipalityModel(ILogger<MunicipalityModel> logger, VotingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var municipality = await _dbContext.FindMunicipalityById(id);
                var sessionList = await _dbContext.LoadSessionsAsync(id);
                this.MunicipalityName = municipality.Name;
                this.SessionList = sessionList.ConvertAll(item => new SessionItem(item));
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Municipality failed");
                return RedirectToPage("Index");
            }
        }
    }
}
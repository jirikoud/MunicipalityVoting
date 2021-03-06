﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VotingCoreData;
using VotingCoreWeb.Models.Session;

namespace VotingCoreWeb
{
    public class SessionModel : PageModel
    {
        private readonly ILogger<SessionModel> _logger;
        private readonly VotingDbContext _dbContext;

        public int MunicipalityId { get; set; }
        public string MunicipalityName { get; set; }
        public int BodyId { get; set; }
        public string BodyName { get; set; }
        public int SessionId { get; set; }
        public string SessionName { get; set; }

        public List<TopicItem> TopicList { get; set; }

        public SessionModel(ILogger<SessionModel> logger, VotingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var session = await _dbContext.GetSessionByIdAsync(id);
                var topicList = await _dbContext.GetTopicListAsync(id);

                this.MunicipalityId = session.Body.MunicipalityId;
                this.MunicipalityName = session.Body.Municipality.Name;
                this.BodyId = session.Body.Id;
                this.BodyName = session.Body.Name;
                this.SessionId = session.Id;
                this.SessionName = session.Name;
                this.TopicList = topicList.ConvertAll(item => new TopicItem(item));
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Session({id}) failed");
                return RedirectToPage("Index");
            }
        }
    }
}
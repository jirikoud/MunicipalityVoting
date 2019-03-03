using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingData;

namespace VotingWeb.Models.Municipalities
{
    public class SessionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public SessionModel(Session entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.StartDate = entity.StartDate.HasValue ? entity.StartDate.Value.ToString("D") : null;
            this.EndDate = entity.StartDate.HasValue ? entity.StartDate.Value.ToString("D") : null;
        }
    }
}
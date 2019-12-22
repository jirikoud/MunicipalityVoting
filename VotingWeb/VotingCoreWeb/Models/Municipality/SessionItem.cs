using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingCoreData.Models;

namespace VotingCoreWeb.Models.Municipality
{
    public class SessionItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public SessionItem(VotingCoreData.Models.Session entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.StartDate = entity.StartDate.HasValue ? entity.StartDate.Value.ToString("D") : null;
            this.EndDate = entity.StartDate.HasValue ? entity.StartDate.Value.ToString("D") : null;
        }
    }
}

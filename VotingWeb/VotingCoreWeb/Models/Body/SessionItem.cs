using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Models.Body
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
            this.StartDate = entity.StartDate?.ToString("D");
            this.EndDate = entity.EndDate?.ToString("D");
        }
    }
}

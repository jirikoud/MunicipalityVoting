using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingCoreData.Models;

namespace VotingCoreWeb.Models.Home
{
    public class BodyItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public BodyItem(VotingCoreData.Models.Body entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.Description = entity.Description;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingCoreData.Models;

namespace VotingCoreWeb.Models.Home
{
    public class MunicipalityItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public MunicipalityItem(VotingCoreData.Models.Municipality entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.Description = entity.Description;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingData;

namespace VotingWeb.Models.Home
{
    public class MunicipalityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public MunicipalityModel(Municipality entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
            this.Description = entity.Description;
        }
    }
}
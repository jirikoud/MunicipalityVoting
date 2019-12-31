using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Models.Api
{
    public class Municipality
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Municipality() { }

        public Municipality(VotingCoreData.Models.Municipality model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Description = model.Description;
        }

        public VotingCoreData.Models.Municipality ToDbModel()
        {
            var model = new VotingCoreData.Models.Municipality()
            {
                Id = this.Id ?? 0,
                Name = this.Name,
                Description = this.Description,
            };
            return model;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Models.Api
{
    public class Body
    {
        public int? Id { get; set; }
        public int MunicipalityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Body() { }

        public Body(VotingCoreData.Models.Body model)
        {
            this.Id = model.Id;
            this.MunicipalityId = model.MunicipalityId;
            this.Name = model.Name;
            this.Description = model.Description;
        }

        public VotingCoreData.Models.Body ToDbModel()
        {
            var model = new VotingCoreData.Models.Body()
            {
                Id = this.Id ?? 0,
                MunicipalityId = this.MunicipalityId,
                Name = this.Name,
                Description = this.Description,
            };
            return model;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Models.Api
{
    public class Party
    {
        public int? Id { get; set; }
        public int MunicipalityId { get; set; }
        public string Name { get; set; }

        public Party() { }

        public Party(VotingCoreData.Models.Party model)
        {
            this.Id = model.Id;
            this.MunicipalityId = model.MunicipalityId;
            this.Name = model.Name;
        }

        public VotingCoreData.Models.Party ToDbModel()
        {
            var model = new VotingCoreData.Models.Party()
            {
                Id = this.Id ?? 0,
                MunicipalityId = this.MunicipalityId,
                Name = this.Name,
            };
            return model;
        }
    }
}

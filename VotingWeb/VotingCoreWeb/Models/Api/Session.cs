using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Models.Api
{
    public class Session
    {
        public int? Id { get; set; }
        public int BodyId { get; set; }
        public string Name { get; set; }
        public string Chairman { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Session() { }

        public Session(VotingCoreData.Models.Session model)
        {
            this.Id = model.Id;
            this.BodyId = model.BodyId;
            this.Name = model.Name;
            this.Chairman = model.Chairman;
            this.StartDate = model.StartDate;
            this.EndDate = model.EndDate;
        }

        public VotingCoreData.Models.Session ToDbModel()
        {
            var model = new VotingCoreData.Models.Session()
            {
                Id = this.Id ?? 0,
                BodyId = this.BodyId,
                Name = this.Name,
                Chairman = this.Chairman,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
            };
            return model;
        }
    }
}

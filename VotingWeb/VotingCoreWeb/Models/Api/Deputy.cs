using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Models.Api
{
    public class Deputy
    {
        public int? Id { get; set; }
        public int MunicipalityId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string TitlePre { get; set; }
        public string TitlePost { get; set; }

        public Deputy() { }

        public Deputy(VotingCoreData.Models.Deputy model)
        {
            this.Id = model.Id;
            this.MunicipalityId = model.MunicipalityId;
            this.Firstname = model.Firstname;
            this.Lastname = model.Lastname;
            this.TitlePre = model.TitlePre;
            this.TitlePost = model.TitlePost;
        }

        public VotingCoreData.Models.Deputy ToDbModel()
        {
            var model = new VotingCoreData.Models.Deputy()
            {
                Id = this.Id ?? 0,
                MunicipalityId = this.MunicipalityId,
                Firstname = this.Firstname,
                Lastname = this.Lastname,
                TitlePre = this.TitlePre,
                TitlePost = this.TitlePost,
            };
            return model;
        }
    }
}

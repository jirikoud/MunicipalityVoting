using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Parties
{
    public class DeleteModel
    {
        public int Id { get; set; }
        public int MunicipalityId { get; set; }

        [Display(Name = "DETAIL_NAME", ResourceType = typeof(PartyAdminRes))]
        public string Name { get; set; }

        public DeleteModel()
        {
        }

        public DeleteModel(Party entity)
        {
            this.Id = entity.Id;
            this.MunicipalityId = entity.MunicipalityId;
            this.Name = entity.Name;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Sessions
{
    public class DeleteModel
    {
        public int Id { get; set; }
        public int MunicipalityId { get; set; }

        [Display(Name = "DETAIL_NAME", ResourceType = typeof(SessionAdminRes))]
        public string Name { get; set; }

        public DeleteModel()
        {
        }

        public DeleteModel(Session entity)
        {
            this.Id = entity.Id;
            this.MunicipalityId = entity.MunicipalityId;
            this.Name = entity.Name;
        }
    }
}
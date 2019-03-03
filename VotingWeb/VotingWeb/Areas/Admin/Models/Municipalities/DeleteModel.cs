using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Municipalities
{
    public class DeleteModel
    {
        public int Id { get; set; }

        [Display(Name = "DETAIL_NAME", ResourceType = typeof(MunicipalityAdminRes))]
        public string Name { get; set; }

        public DeleteModel()
        {
        }

        public DeleteModel(Municipality entity)
        {
            this.Id = entity.Id;
            this.Name = entity.Name;
        }
    }
}
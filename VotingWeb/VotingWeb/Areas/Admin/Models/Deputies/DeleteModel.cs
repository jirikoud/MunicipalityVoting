using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Deputies
{
    public class DeleteModel
    {
        public int Id { get; set; }
        public int MunicipalityId { get; set; }

        [Display(Name = "DETAIL_FULLNAME", ResourceType = typeof(DeputyAdminRes))]
        public string FullName { get; set; }

        public DeleteModel()
        {
        }

        public DeleteModel(Deputy entity)
        {
            this.Id = entity.Id;
            this.MunicipalityId = entity.MunicipalityId;
            this.FullName = entity.GetFullName();
        }
    }
}
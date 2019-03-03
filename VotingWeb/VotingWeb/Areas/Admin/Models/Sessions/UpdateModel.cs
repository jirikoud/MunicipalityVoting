using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Sessions
{
    public class UpdateModel
    {
        public int Id { get; set; }
        public bool IsCreate { get; set; }
        public int MunicipalityId { get; set; }

        [Display(Name = "DETAIL_NAME", ResourceType = typeof(SessionAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string Name { get; set; }

        [Display(Name = "DETAIL_CHAIRMAN", ResourceType = typeof(SessionAdminRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(AdminRes))]
        public string Chairman { get; set; }

        [Display(Name = "DETAIL_DATE_START", ResourceType = typeof(SessionAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        public DateTime? DateStart { get; set; }

        [Display(Name = "DETAIL_DATE_END", ResourceType = typeof(SessionAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        public DateTime? DateEnd { get; set; }

        public string DateStartFormat
        {
            get
            {
                if (this.DateStart.HasValue)
                {
                    return this.DateStart.Value.ToString("yyyy-MM-dd");
                }
                return null;
            }
        }

        public string DateEndFormat
        {
            get
            {
                if (this.DateEnd.HasValue)
                {
                    return this.DateEnd.Value.ToString("yyyy-MM-dd");
                }
                return null;
            }
        }


        public UpdateModel()
        {
        }

        public UpdateModel(Session entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.MunicipalityId = entity.MunicipalityId;
                this.Name = entity.Name;
                this.Chairman = entity.Chairman;
                this.DateStart = entity.StartDate;
                this.DateEnd = entity.EndDate;
            }
            else
            {
                this.IsCreate = true;
            }
        }
    }
}
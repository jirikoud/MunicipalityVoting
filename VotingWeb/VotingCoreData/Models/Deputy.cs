﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VotingCoreData.Properties;

namespace VotingCoreData.Models
{
    public class Deputy
    {
        public int Id { get; set; }
        public int MunicipalityId { get; set; }

        [Display(Name = "DEPUTY_FIRSTNAME", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
        public string Firstname { get; set; }

        [Display(Name = "DEPUTY_LASTNAME", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
        public string Lastname { get; set; }

        [Display(Name = "DEPUTY_TITLE_PRE", ResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
        public string TitlePre { get; set; }

        [Display(Name = "DEPUTY_TITLE_POST", ResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
        public string TitlePost { get; set; }

        [Display(Name = "DEPUTY_PARTY", ResourceType = typeof(ModelRes))]
        public int? PartyId { get; set; }

        public bool IsDeleted { get; set; }

        public Municipality Municipality { get; set; }
        public List<Voting> Votings { get; set; }
        public Party Party { get; set; }

        public string GetFullName()
        {
            var fullname = this.Firstname + " " + this.Lastname;
            if (!string.IsNullOrWhiteSpace(this.TitlePre))
            {
                fullname = this.TitlePre + " " + fullname;
            }
            if (!string.IsNullOrWhiteSpace(this.TitlePost))
            {
                fullname = fullname + " " + this.TitlePost;
            }
            return fullname;
        }

        public void UpdateFrom(Deputy model)
        {
            this.Firstname = model.Firstname;
            this.Lastname = model.Lastname;
            this.TitlePre = model.TitlePre;
            this.TitlePost = model.TitlePost;
            this.PartyId = model.PartyId;
        }
    }
}

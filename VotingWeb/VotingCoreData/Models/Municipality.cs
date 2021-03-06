﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VotingCoreData.Properties;

namespace VotingCoreData.Models
{
    public class Municipality
    {
        public int Id { get; set; }

        [Display(Name = "MUNICIPALITY_NAME", ResourceType = typeof(ModelRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
        public string Name { get; set; }

        [Display(Name = "MUNICIPALITY_DESCRIPTION", ResourceType = typeof(ModelRes))]
        [StringLength(255, ErrorMessageResourceName = "VALIDATION_LENGTH", ErrorMessageResourceType = typeof(ModelRes))]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public List<Deputy> Deputies { get; set; }
        public List<Party> Parties { get; set; }
        public List<Body> Bodies { get; set; }

        public void UpdateFrom(Municipality model)
        {
            this.Name = model.Name;
            this.Description = model.Description;
        }
    }
}

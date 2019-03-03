using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingCommon.Models;

namespace VotingWeb.Areas.Admin.Models.Imports
{
    public class ImportModel
    {
        public int MunicipalityId { get; set; }

        public int Importer { get; set; }
        public SelectList ImporterList { get; set; }

        public string Filename { get; set; }
        public SessionModel SessionModel { get; set; }
    }
}
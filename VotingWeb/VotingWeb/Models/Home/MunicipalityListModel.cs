using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingData;

namespace VotingWeb.Models.Home
{
    public class MunicipalityListModel
    {
        public List<MunicipalityModel> MunicipalityList { get; set; }

        public MunicipalityListModel(List<Municipality> municipalityList)
        {
            this.MunicipalityList = municipalityList.ConvertAll(item => new MunicipalityModel(item));
        }
    }
}
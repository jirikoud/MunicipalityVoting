using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotingWeb.Models.Municipalities
{
    public class SessionListModel
    {
        public string MunicipalityName { get; set; }
        public List<SessionModel> SessionList { get; set; }

        public SessionListModel(VotingData.Municipality municipality, List<VotingData.Session> sessionList)
        {
            this.MunicipalityName = municipality.Name;
            this.SessionList = sessionList.ConvertAll(item => new SessionModel(item));
        }
    }
}
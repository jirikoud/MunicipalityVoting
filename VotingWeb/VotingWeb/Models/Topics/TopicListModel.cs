using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingData;

namespace VotingWeb.Models.Topics
{
    public class TopicListModel
    {
        public int MunicipalityId { get; set; }
        public string MunicipalityName { get; set; }
        public int SessionId { get; set; }
        public string SessionName { get; set; }

        public List<TopicModel> TopicList { get; set; }

        public TopicListModel(Session session, List<Topic> topicList)
        {
            this.MunicipalityId = session.MunicipalityId;
            this.MunicipalityName = session.Municipality.Name;
            this.SessionId = session.Id;
            this.SessionName = session.Name;
            this.TopicList = topicList.ConvertAll(item => new TopicModel(item));
        }
    }
}
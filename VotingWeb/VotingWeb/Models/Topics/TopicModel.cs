using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Infrastructure;

namespace VotingWeb.Models.Topics
{
    public class TopicModel
    {
        public int Id { get; set; }
        public string Order { get; set; }
        //public string ShortName { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }

        public TopicModel(Topic entity)
        {
            this.Id = entity.Id;
            this.Order = entity.Order.ToString();
            //this.ShortName = ContextUtils.Instance.ShortenString(entity.Name, 40);
            this.Name = entity.Name;
            this.Comment = entity.Comment;
        }
    }
}
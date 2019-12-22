using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingCoreData.Models;

namespace VotingCoreWeb.Models.Session
{
    public class TopicItem
    {
        public int Id { get; set; }
        public string Order { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }

        public TopicItem(VotingCoreData.Models.Topic entity)
        {
            this.Id = entity.Id;
            this.Order = entity.Order.ToString();
            this.Name = entity.Name;
            this.Comment = entity.Comment;
        }
    }
}

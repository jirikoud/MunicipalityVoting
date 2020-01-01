using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Models.Api
{
    public class Topic
    {
        public int? Id { get; set; }
        public int SessionId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public DateTime? Time { get; set; }
        public bool IsProcedural { get; set; }
        public bool IsSecret { get; set; }
        public bool IsApproved { get; set; }


        public Topic() { }

        public Topic(VotingCoreData.Models.Topic model)
        {
            this.Id = model.Id;
            this.SessionId = model.SessionId;
            this.Name = model.Name;
            this.Comment = model.Comment;
            this.Text = model.Text;
            this.Order = model.Order;
            this.Time = model.Time;
            this.IsProcedural = model.IsProcedural;
            this.IsSecret = model.IsSecret;
            this.IsApproved = model.IsApproved;
        }

        public VotingCoreData.Models.Topic ToDbModel()
        {
            var model = new VotingCoreData.Models.Topic()
            {
                Id = this.Id ?? 0,
                SessionId = this.SessionId,
                Name = this.Name,
                Comment = this.Comment,
                Text = this.Text,
                Order = this.Order,
                Time = this.Time,
                IsProcedural = this.IsProcedural,
                IsSecret = this.IsSecret,
                IsApproved = this.IsApproved,
            };
            return model;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingCommon.Models
{
    public class SessionModel
    {
        public string ErrorMessage { get; set; }

        public string Title { get; set; }
        public string Comment { get; set; }
        public string Chairman { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<TopicModel> TopicList { get; set; }
    }
}

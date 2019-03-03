using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingCommon.Models
{
    public class TopicModel
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public DateTime? Time { get; set; }
        public int DeputyTotal { get; set; }
        public bool IsProcedural { get; set; }
        public bool IsSecret { get; set; }
        public int Order { get; set; }

        public List<DeputyModel> DeputyList { get; set; }
    }
}

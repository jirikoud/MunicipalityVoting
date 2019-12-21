using System;
using System.Collections.Generic;
using System.Text;

namespace VotingCoreData.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Chairman { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MunicipalityId { get; set; }
        public bool IsDeleted { get; set; }

        public Municipality Municipality { get; set; }
        public List<Topic> Topics { get; set; }
    }
}

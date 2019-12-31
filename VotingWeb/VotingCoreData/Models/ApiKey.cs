using System;
using System.Collections.Generic;
using System.Text;

namespace VotingCoreData.Models
{
    public class ApiKey
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string AspNetUserId { get; set; }

        public User AspNetUser { get; set; }
    }
}

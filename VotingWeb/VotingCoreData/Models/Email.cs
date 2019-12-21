using System;
using System.Collections.Generic;
using System.Text;

namespace VotingCoreData.Models
{
    public class Email
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Receiver { get; set; }
        public string Body { get; set; }
        public bool IsSent { get; set; }
        public int ErrorCount { get; set; }
        public string ErrorMessage { get; set; }
    }
}

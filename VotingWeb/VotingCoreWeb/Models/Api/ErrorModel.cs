using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Models.Api
{
    public class ErrorModel
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public string DebugMessage { get; set; }
    }
}

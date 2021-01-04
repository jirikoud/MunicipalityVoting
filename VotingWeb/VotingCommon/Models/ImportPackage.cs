using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCommon.Models
{
    public class ImportPackage
    {
        public List<SessionModel> Sessions { get; set; }

        public string ErrorMessage { get; set; }

        public ImportPackage(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public ImportPackage(SessionModel sessionModel)
        {
            this.Sessions = new List<SessionModel>() { sessionModel };
        }
    }
}

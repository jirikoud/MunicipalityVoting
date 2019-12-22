using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Infrastructure
{
    public class AlertModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Class { get; set; }

        public AlertModel(AlertTypeEnum alertType, string message)
        {
            this.Class = AlertTypeConvertor.GetClass(alertType);
            this.Title = AlertTypeConvertor.GetTitle(alertType);
            this.Message = message;
        }
    }
}

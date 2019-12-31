using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Areas.Identity.Pages.Account.Manage
{
    [Serializable]
    public class StatusMessageModel
    {
        public string Message { get; set; }
        public bool IsError { get; set; }

        public static string Create(string message, bool isError = false)
        {
            var data = new StatusMessageModel()
            {
                IsError = isError,
                Message = message,
            };
            return JsonConvert.SerializeObject(data);
        }

        public static StatusMessageModel Decode(string serialized)
        {
            if (serialized == null)
            {
                return null;
            }
            var data = JsonConvert.DeserializeObject<StatusMessageModel>(serialized);
            return data;
        }
    }
}

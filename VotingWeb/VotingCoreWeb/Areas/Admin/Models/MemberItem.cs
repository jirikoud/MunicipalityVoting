using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingCoreData.Models;

namespace VotingCoreWeb.Areas.Admin.Models
{
    public class MemberItem
    {
        public int DeputyId { get; set; }
        public string DeputyName { get; set; }
        public bool IsChecked { get; set; }

        public MemberItem()
        {

        }

        public MemberItem(Deputy model, bool isChecked)
        {
            this.DeputyId = model.Id;
            this.DeputyName = model.GetFullName();
            this.IsChecked = isChecked;
        }
    }
}

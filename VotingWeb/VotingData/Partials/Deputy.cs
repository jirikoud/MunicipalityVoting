using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingData
{
    public partial class Deputy
    {
        public string GetFullName()
        {
            var fullname = this.Firstname + " " + this.Lastname;
            if (!string.IsNullOrWhiteSpace(this.TitlePre))
            {
                fullname = this.TitlePre + " " + fullname;
            }
            if (!string.IsNullOrWhiteSpace(this.TitlePost))
            {
                fullname = fullname + " " + this.TitlePost;
            }
            return fullname;
        }
    }
}

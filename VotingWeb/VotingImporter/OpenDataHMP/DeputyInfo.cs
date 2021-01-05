using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingImporter.OpenDataHMP
{
    public class DeputyInfo
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string TitlePost { get; set; }
        public string TitlePre { get; set; }
        public int ColumnIndex { get; set; }
    }
}

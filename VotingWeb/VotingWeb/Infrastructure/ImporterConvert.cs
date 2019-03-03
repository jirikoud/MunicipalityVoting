using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingImporter;
using VotingWeb.Properties;

namespace VotingWeb.Infrastructure
{
    public class ImporterConvert
    {
        private static List<Tuple<int, string>> importerList = new List<Tuple<int, string>>() {
            new Tuple<int, string>((int)ImporterEnum.BitEST, CommonRes.IMPORTER_BITEST),
        };

        public static List<Tuple<int, string>> GetImporterList()
        {
            return importerList;
        }

    }
}
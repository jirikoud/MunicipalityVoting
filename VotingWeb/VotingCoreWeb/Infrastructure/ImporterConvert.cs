using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingImporter;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Infrastructure
{
    public class ImporterConvert
    {
        private static List<Tuple<int, string>> importerList = new List<Tuple<int, string>>() {
            new Tuple<int, string>((int)ImporterEnum.BitEST, CommonRes.IMPORTER_BITEST),
            new Tuple<int, string>((int)ImporterEnum.OpenDataHMP, CommonRes.IMPORTER_OPEN_DATA_HMP),
        };

        public static List<Tuple<int, string>> GetImporterList()
        {
            return importerList;
        }

    }
}
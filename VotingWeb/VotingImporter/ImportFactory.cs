using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingImporter.BitEST;
using VotingImporter.OpenDataHMP;

namespace VotingImporter
{
    public class ImportFactory
    {
        public static IVotingImporter GetImporter(ImporterEnum importer)
        {
            switch (importer)
            {
                case ImporterEnum.BitEST:
                    return new BitESTImporter();
                case ImporterEnum.OpenDataHMP:
                    return new OpenDataImporter();
            }
            return null;
        }
    }
}

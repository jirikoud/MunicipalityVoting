using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingImporter.BitEST;

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
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingCommon.Models;

namespace VotingImporter
{
    public interface IVotingImporter
    {
        ImportPackage ImportFromFile(string filename);
    }
}

using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VotingCommon.Models;
using VotingImporter.Properties;

namespace VotingImporter.OpenDataHMP
{
    public class OpenDataImporter : IVotingImporter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ImportPackage ImportFromFile(string filename)
        {
            var model = new SessionModel();
            if (string.IsNullOrWhiteSpace(filename))
            {
                return new ImportPackage(Resources.ERROR_NO_FILE);
            }
            if (!File.Exists(filename))
            {
                return new ImportPackage(Resources.ERROR_FILE_NOT_FOUND);
            }
            try
            {
                CultureInfo cultureInfo = CultureInfo.InvariantCulture;
                var allLines = File.ReadAllLines(filename);
                if (allLines.Length < 2)
                {
                    return new ImportPackage(Resources.ERROR_FILE_EMPTY);
                }
                var headerLine = allLines[0];
                var headerFields = headerLine.Split(';');
                var deputyNames = new List<string>();
                for (int index = 17; index < headerFields.Length; index++)
                {
                    deputyNames.Add(headerFields[index]);
                }

                logger.Debug("Opening file '{0}' ...", filename);
                return new ImportPackage(model);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return new ImportPackage(Resources.ERROR_PARSE);
            }
        }
    }
}

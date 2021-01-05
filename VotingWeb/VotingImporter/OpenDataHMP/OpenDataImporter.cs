using CsvHelper;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VotingCommon.Enumerations;
using VotingCommon.Models;
using VotingImporter.Properties;

namespace VotingImporter.OpenDataHMP
{
    public class OpenDataImporter : IVotingImporter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private DeputyInfo ParseDeputy(string fullname, int columnIndex)
        {
            var deputy = new DeputyInfo() { ColumnIndex = columnIndex };
            var nameSplit = fullname.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            deputy.Firstname = nameSplit[1];
            deputy.Lastname = nameSplit[0];
            var preList = new List<string>();
            var postList = new List<string>();
            for (int index = 2; index < nameSplit.Length; index++)
            {
                switch (nameSplit[index])
                {
                    case "Ph.D.":
                        postList.Add(nameSplit[index]);
                        break;
                    case "Th.D.":
                        postList.Add(nameSplit[index]);
                        break;
                    case "CSc.":
                        postList.Add(nameSplit[index]);
                        break;
                    case "DrSc.":
                        postList.Add(nameSplit[index]);
                        break;
                    case "DiS.":
                        postList.Add(nameSplit[index]);
                        break;
                    case "MSc.":
                        postList.Add(nameSplit[index]);
                        break;
                    default:
                        preList.Add(nameSplit[index]);
                        break;
                }
            }
            if (preList.Count > 0)
            {
                deputy.TitlePre = string.Join(" ", preList);
            }
            if (postList.Count > 0)
            {
                deputy.TitlePost = string.Join(", ", postList);
            }
            return deputy;
        }

        private VoteEnum? ParseVote(string value)
        {
            switch (value)
            {
                case "Hlas pro":
                    return VoteEnum.Yes;
                case "Nehlasoval":
                    return VoteEnum.NotVoting;
                case "Chyběl":
                    return VoteEnum.Missing;
                case "Zdržel se":
                    return VoteEnum.Abstain;
                case "Hlas proti":
                    return VoteEnum.No;
                default:
                    return null;
            }
        }

        public ImportPackage ImportFromFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return new ImportPackage(Resources.ERROR_NO_FILE);
            }
            if (!File.Exists(filename))
            {
                return new ImportPackage(Resources.ERROR_FILE_NOT_FOUND);
            }
            int lineIndex = 1;
            try
            {
                logger.Debug("Opening file '{0}' ...", filename);
                CultureInfo cultureInfo = CultureInfo.InvariantCulture;
                var sessions = new List<SessionModel>();

                using (var reader = new StreamReader(filename))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Configuration.Delimiter = ";";
                        csv.Configuration.BadDataFound = (context) => 
                        {
                            logger.Warn($"Bad import data: {context.RawRecord}");
                        };
                        csv.Read();
                        csv.ReadHeader();

                        var deputies = new List<DeputyInfo>();
                        for (int index = 17; index < csv.Context.HeaderRecord.Length; index++)
                        {
                            if (!string.IsNullOrWhiteSpace(csv.Context.HeaderRecord[index]))
                            {
                                var deputy = ParseDeputy(csv.Context.HeaderRecord[index], index);
                                deputies.Add(deputy);
                            }
                        }

                        SessionModel currentSession = null;
                        while (csv.Read())
                        {
                            var title = csv.GetField(6);

                            var date = DateTime.ParseExact(csv.GetField(3), "dd.MM.yyyy H:mm", cultureInfo);
                            if (currentSession == null || currentSession.StartDate != date)
                            {
                                if (currentSession != null)
                                {
                                    currentSession.TopicList = currentSession.TopicList.OrderBy(item => item.Order).ToList();
                                }
                                currentSession = new SessionModel()
                                {
                                    StartDate = date,
                                    Title = $"{csv.GetField(9)}. zasedání ZHMP",
                                    EndDate = date,
                                    Comment = $"Datum: {date:d.M.yyyy}\nVolební období: {csv.GetField(2)}",
                                    TopicList = new List<TopicModel>()
                                };
                                sessions.Add(currentSession);
                            }
                            if (string.IsNullOrWhiteSpace(csv.GetField(14)))
                            {
                                logger.Warn("Invalid import data - line {lineIndex}", lineIndex + 1);
                            }
                            else
                            {
                                var votesYes = int.Parse(csv.GetField(14));
                                var topic = new TopicModel()
                                {
                                    Name = csv.GetField(6),
                                    Order = int.Parse(csv.GetField(8)),
                                    Time = DateTime.ParseExact(csv.GetField(10), "dd.MM.yyyy H:mm", cultureInfo),
                                    Comment = $"Číslo usnesení: {csv.GetField(1)}\nČíslo tisku: {csv.GetField(0)}\nPředkladatel: {csv.GetField(7)}",
                                    IsApproved = votesYes > 32,
                                    DeputyList = new List<DeputyModel>(),
                                };
                                currentSession.TopicList.Add(topic);
                                foreach (var deputyInfo in deputies)
                                {
                                    var vote = ParseVote(csv.GetField(deputyInfo.ColumnIndex));
                                    if (vote.HasValue)
                                    {
                                        topic.DeputyList.Add(new DeputyModel()
                                        {
                                            FirstName = deputyInfo.Firstname,
                                            Lastname = deputyInfo.Lastname,
                                            TitlePre = deputyInfo.TitlePre,
                                            TitlePost = deputyInfo.TitlePost,
                                            Vote = vote.Value,
                                        });
                                    }
                                }
                            }
                        }
                        if (currentSession != null)
                        {
                            currentSession.TopicList = currentSession.TopicList.OrderBy(item => item.Order).ToList();
                        }
                    }
                }
                if (sessions.Count == 0)
                {
                    return new ImportPackage(Resources.ERROR_FILE_EMPTY);
                }
                return new ImportPackage(sessions);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return new ImportPackage(Resources.ERROR_PARSE);
            }
        }
    }
}

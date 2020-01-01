using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VotingCommon.Enumerations;
using VotingCommon.Models;
using VotingImporter.Properties;

namespace VotingImporter.BitEST
{
    public class BitESTImporter : IVotingImporter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public SessionModel ImportFromFile(string filename)
        {
            var model = new SessionModel();
            if (string.IsNullOrWhiteSpace(filename))
            {
                model.ErrorMessage = Resources.ERROR_NO_FILE;
                return model;
            }
            if (!File.Exists(filename))
            {
                model.ErrorMessage = Resources.ERROR_FILE_NOT_FOUND;
                return model;
            }
            try
            {
                CultureInfo cultureInfo = CultureInfo.InvariantCulture;
                logger.Debug("Opening file '{0}' ...", filename);
                var elementRoot = XElement.Load(filename);
                model.Chairman = elementRoot.Attribute("chairman").Value;
                model.StartDate = DateTime.ParseExact(elementRoot.Attribute("start_date").Value, "dd.MM.yyyy", cultureInfo);
                model.EndDate = DateTime.ParseExact(elementRoot.Attribute("end_date").Value, "dd.MM.yyyy", cultureInfo);
                var elementTitle = elementRoot.Element("Title");
                model.Title = elementTitle.Value;
                var elementComment = elementRoot.Element("Comment");
                model.Comment = elementComment.Value;

                model.TopicList = new List<TopicModel>();
                var elementListVoting = elementRoot.Elements("VotingResult");
                foreach (var elementVoting in elementListVoting)
                {
                    var modelTopic = new TopicModel();
                    model.TopicList.Add(modelTopic);

                    var yesVote = int.Parse(elementVoting.Attribute("aye").Value);
                    var deputyTotal = int.Parse(elementVoting.Attribute("deputies").Value);
                    var appproveNeeded = (deputyTotal / 2) + 1;

                    modelTopic.Order = int.Parse(elementVoting.Attribute("number").Value);
                    modelTopic.Time = DateTime.ParseExact(elementVoting.Attribute("time").Value, "dd.MM.yyyy HH:mm:ss", cultureInfo);
                    modelTopic.IsProcedural = (elementVoting.Attribute("is_procedural").Value == "Yes");
                    modelTopic.IsSecret = (elementVoting.Attribute("is_secret").Value == "Yes");
                    modelTopic.IsApproved = (yesVote >= appproveNeeded);
                    var elementTopic = elementVoting.Element("Topic");
                    modelTopic.Name = elementTopic.Value;
                    var elementTopicComment = elementVoting.Element("Comment");
                    modelTopic.Comment = elementTopicComment.Value;
                    modelTopic.DeputyList = new List<DeputyModel>();

                    var elementListDeputy = elementVoting.Elements("Deputy");
                    foreach (var elementDeputy in elementListDeputy)
                    {
                        var modelDeputy = new DeputyModel();
                        modelDeputy.FirstName = elementDeputy.Attribute("first_name").Value;
                        modelDeputy.Lastname = elementDeputy.Attribute("name").Value;
                        modelDeputy.TitlePre = elementDeputy.Attribute("title").Value;
                        modelDeputy.Party = elementDeputy.Attribute("party").Value;
                        modelDeputy.Vote = VoteDecoder.Decode(elementDeputy.Attribute("vote").Value).Value;
                        modelTopic.DeputyList.Add(modelDeputy);
                    }
                }

                return model;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                model.ErrorMessage = Resources.ERROR_XML_PARSE;
                return model;
            }
        }
    }
}

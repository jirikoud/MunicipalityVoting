using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingCommon.Models;
using VotingData;
using VotingWeb.Areas.Admin.Models.Municipalities;

namespace VotingWeb.DataContext
{
    public class MunicipalityContext
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Instance

        private static readonly MunicipalityContext instance = new MunicipalityContext();

        public static MunicipalityContext Instance { get { return instance; } }

        private MunicipalityContext() { }

        #endregion

        public List<Municipality> LoadList(VotingEntities entities)
        {
            try
            {
                var list = entities.Municipalities.Where(item => !item.IsDeleted).OrderBy(item => item.Name).ToList();
                return list;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public Municipality FindById(VotingEntities entities, int id)
        {
            try
            {
                var entity = entities.Municipalities.FirstOrDefault(item => !item.IsDeleted && item.Id == id);
                return entity;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public int? Update(VotingEntities entities, Municipality entity, UpdateModel model)
        {
            try
            {
                if (entity == null)
                {
                    entity = new Municipality();
                    entities.Municipalities.Add(entity);
                }
                entity.Name = model.Name;
                entity.Description = model.Description;
                entities.SaveChanges();
                return entity.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public bool Delete(VotingEntities entities, Municipality entity)
        {
            try
            {
                entity.IsDeleted = true;
                entities.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return false;
            }
        }

        public bool ImportSession(VotingEntities entities, Municipality municipality, SessionModel sessionModel)
        {
            var session = new Session()
            {
                Name = sessionModel.Title,
                Chairman = sessionModel.Chairman,
                StartDate = sessionModel.StartDate,
                EndDate = sessionModel.EndDate,
                Municipality = municipality,
            };
            entities.Sessions.Add(session);

            var deputyDictionary = entities.Deputies.Where(item => item.MunicipalityId == municipality.Id && !item.IsDeleted).ToDictionary(item => item.TitlePre + ";" + item.Firstname + ";" + item.Lastname + ";" + item.TitlePost, item => item);
            var partyDictionary = entities.Parties.Where(item => item.MunicipalityId == municipality.Id && !item.IsDeleted).ToDictionary(item => item.Name, item => item);

            foreach (var topicModel in sessionModel.TopicList)
            {
                var topic = new Topic()
                {
                    Name = topicModel.Name,
                    Comment = topicModel.Comment,
                    Order = topicModel.Order,
                    Time = topicModel.Time,
                    Total = topicModel.DeputyTotal,
                    IsProcedural = topicModel.IsProcedural,
                    IsSecret = topicModel.IsSecret,
                };
                session.Topics.Add(topic);
                foreach (var deputyModel in topicModel.DeputyList)
                {
                    if (!partyDictionary.ContainsKey(deputyModel.Party))
                    {
                        var newParty = new Party()
                        {
                            MunicipalityId = municipality.Id,
                            Name = deputyModel.Party,
                        };
                        entities.Parties.Add(newParty);
                        partyDictionary.Add(newParty.Name, newParty);
                    }
                    var party = partyDictionary[deputyModel.Party];

                    var deputyKey = deputyModel.TitlePre + ";" + deputyModel.FirstName + ";" + deputyModel.Lastname + ";" + deputyModel.TitlePost;
                    if (!deputyDictionary.ContainsKey(deputyKey))
                    {
                        var newDeputy = new Deputy()
                        {
                            MunicipalityId = municipality.Id,
                            Firstname = deputyModel.FirstName,
                            Lastname = deputyModel.Lastname,
                            TitlePre = deputyModel.TitlePre,
                            TitlePost = deputyModel.TitlePost,
                        };
                        entities.Deputies.Add(newDeputy);
                        deputyDictionary.Add(deputyKey, newDeputy);
                    }
                    var deputy = deputyDictionary[deputyKey];

                    var voting = new Voting()
                    {
                        Deputy = deputy,
                        Party = party,
                        Topic = topic,
                        Vote = (int)deputyModel.Vote,
                    };
                    entities.Votings.Add(voting);
                }
            }
            entities.SaveChanges();
            return true;
        }
    }
}
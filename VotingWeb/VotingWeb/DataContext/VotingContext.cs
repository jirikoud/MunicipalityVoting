using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Areas.Admin.Models.Votings;

namespace VotingWeb.DataContext
{
    public class VotingContext
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Instance

        private static readonly VotingContext instance = new VotingContext();

        public static VotingContext Instance { get { return instance; } }

        private VotingContext() { }

        #endregion

        public List<Voting> LoadList(VotingEntities entities, int topicId)
        {
            try
            {
                var list = entities.Votings.Where(item => item.TopicId == topicId).ToList();
                return list;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public bool Exists(VotingEntities entities, int topicId, int deputyId)
        {
            try
            {
                var exists = entities.Votings.Any(item => item.TopicId == topicId && item.DeputyId == deputyId);
                return exists;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return false;
            }
        }

        public Voting FindById(VotingEntities entities, int id)
        {
            try
            {
                var entity = entities.Votings.FirstOrDefault(item => item.Id == id);
                return entity;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public int? Update(VotingEntities entities, Voting entity, UpdateModel model)
        {
            try
            {
                if (entity == null)
                {
                    entity = new Voting();
                    entity.TopicId = model.TopicId;
                    entities.Votings.Add(entity);
                }
                entity.DeputyId = model.DeputyId;
                entity.PartyId = model.PartyId;
                entity.Vote = model.Vote;
                entities.SaveChanges();
                return entity.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public bool Delete(VotingEntities entities, Voting entity)
        {
            try
            {
                entities.Votings.Remove(entity);
                entities.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return false;
            }
        }

    }
}
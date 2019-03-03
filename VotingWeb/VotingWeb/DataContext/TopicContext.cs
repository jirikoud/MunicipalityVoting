using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Areas.Admin.Models.Topics;

namespace VotingWeb.DataContext
{
    public class TopicContext
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Instance

        private static readonly TopicContext instance = new TopicContext();

        public static TopicContext Instance { get { return instance; } }

        private TopicContext() { }

        #endregion

        public List<Topic> LoadList(VotingEntities entities, int sessionId)
        {
            try
            {
                var list = entities.Topics.Where(item => !item.IsDeleted && item.SessionId == sessionId).OrderBy(item => item.Order).ToList();
                return list;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public int? GetCountInSession(VotingEntities entities, int sessionId)
        {
            try
            {
                var count = entities.Topics.Where(item => !item.IsDeleted && item.SessionId == sessionId).Count();
                return count;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public Topic FindById(VotingEntities entities, int id)
        {
            try
            {
                var entity = entities.Topics.FirstOrDefault(item => !item.IsDeleted && item.Id == id);
                return entity;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public int? Update(VotingEntities entities, Topic entity, UpdateModel model)
        {
            try
            {
                if (entity == null)
                {
                    entity = new Topic();
                    entity.SessionId = model.SessionId;
                    entities.Topics.Add(entity);
                }
                entity.Order = model.Order;
                entity.Name = model.Name;
                entity.Comment = model.Comment;
                entity.IsProcedural = model.IsProcedural;
                entity.IsSecret = model.IsSecret;
                entities.SaveChanges();
                return entity.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public bool Delete(VotingEntities entities, Topic entity)
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

    }
}
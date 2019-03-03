using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Areas.Admin.Models.Sessions;

namespace VotingWeb.DataContext
{
    public class SessionContext
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Instance

        private static readonly SessionContext instance = new SessionContext();

        public static SessionContext Instance { get { return instance; } }

        private SessionContext() { }

        #endregion

        public List<Session> LoadList(VotingEntities entities, int municipalityId)
        {
            try
            {
                var list = entities.Sessions.Where(item => !item.IsDeleted && item.MunicipalityId == municipalityId).OrderByDescending(item => item.StartDate).ToList();
                return list;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public Session FindById(VotingEntities entities, int id)
        {
            try
            {
                var entity = entities.Sessions.FirstOrDefault(item => !item.IsDeleted && item.Id == id);
                return entity;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public int? Update(VotingEntities entities, Session entity, UpdateModel model)
        {
            try
            {
                if (entity == null)
                {
                    entity = new Session();
                    entity.MunicipalityId = model.MunicipalityId;
                    entities.Sessions.Add(entity);
                }
                entity.Name = model.Name;
                entity.Chairman = model.Chairman;
                entity.StartDate = model.DateStart;
                entity.EndDate = model.DateEnd;
                entities.SaveChanges();
                return entity.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public bool Delete(VotingEntities entities, Session entity)
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
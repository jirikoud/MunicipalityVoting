using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Areas.Admin.Models.Deputies;

namespace VotingWeb.DataContext
{
    public class DeputyContext
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Instance

        private static readonly DeputyContext instance = new DeputyContext();

        public static DeputyContext Instance { get { return instance; } }

        private DeputyContext() { }

        #endregion

        public List<Deputy> LoadList(VotingEntities entities, int municipalityId)
        {
            try
            {
                var list = entities.Deputies.Where(item => !item.IsDeleted && item.MunicipalityId == municipalityId).OrderBy(item => item.Lastname).ThenBy(item => item.Firstname).ToList();
                return list;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public Deputy FindById(VotingEntities entities, int id)
        {
            try
            {
                var entity = entities.Deputies.FirstOrDefault(item => !item.IsDeleted && item.Id == id);
                return entity;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public int? Update(VotingEntities entities, Deputy entity, UpdateModel model)
        {
            try
            {
                if (entity == null)
                {
                    entity = new Deputy();
                    entity.MunicipalityId = model.MunicipalityId;
                    entities.Deputies.Add(entity);
                }
                entity.Firstname = model.Firstname;
                entity.Lastname = model.Lastname;
                entity.TitlePre = model.TitlePre;
                entity.TitlePost = model.TitlePost;
                entities.SaveChanges();
                return entity.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public bool Delete(VotingEntities entities, Deputy entity)
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
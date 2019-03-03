using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingData;
using VotingWeb.Areas.Admin.Models.Parties;

namespace VotingWeb.DataContext
{
    public class PartyContext
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Instance

        private static readonly PartyContext instance = new PartyContext();

        public static PartyContext Instance { get { return instance; } }

        private PartyContext() { }

        #endregion

        public List<Party> LoadList(VotingEntities entities, int municipalityId)
        {
            try
            {
                var list = entities.Parties.Where(item => !item.IsDeleted && item.MunicipalityId == municipalityId).OrderBy(item => item.Name).ToList();
                return list;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public Party FindById(VotingEntities entities, int id)
        {
            try
            {
                var entity = entities.Parties.FirstOrDefault(item => !item.IsDeleted && item.Id == id);
                return entity;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public int? Update(VotingEntities entities, Party entity, UpdateModel model)
        {
            try
            {
                if (entity == null)
                {
                    entity = new Party();
                    entity.MunicipalityId = model.MunicipalityId;
                    entities.Parties.Add(entity);
                }
                entity.Name = model.Name;
                entities.SaveChanges();
                return entity.Id;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }

        public bool Delete(VotingEntities entities, Party entity)
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
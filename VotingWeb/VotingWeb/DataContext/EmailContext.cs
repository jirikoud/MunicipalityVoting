using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingData;

namespace VotingWeb.DataContext
{
    public class EmailContext
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Instance

        private static readonly EmailContext instance = new EmailContext();

        public static EmailContext Instance { get { return instance; } }

        private EmailContext() { }

        #endregion

        public bool Create(VotingEntities entities, string subject, string receiver, string body)
        {
            try
            {
                var entity = new Email();
                entity.Subject = subject;
                entity.Receiver = receiver;
                entity.Body = body;
                entities.Emails.Add(entity);
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
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingData;

namespace VotingWeb.DataContext
{
    public class ShortcutContext
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Instance

        private static readonly ShortcutContext instance = new ShortcutContext();

        public static ShortcutContext Instance { get { return instance; } }

        private ShortcutContext() { }

        #endregion

        public Shortcut FindByHash(VotingEntities entities, string hash)
        {
            try
            {
                var entity = entities.Shortcuts.FirstOrDefault(item => item.Hash == hash && !item.Topic.IsDeleted);
                return entity;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
            }
        }
    }
}
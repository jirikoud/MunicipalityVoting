using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingData;
using VotingWeb.DataContext;
using VotingWeb.Models.Topics;

namespace VotingWeb.Controllers
{
    public class SessionController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Session
        public ActionResult Index(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var session = SessionContext.Instance.FindById(entities, id);
                    var topicList = TopicContext.Instance.LoadList(entities, id);
                    var model = new TopicListModel(session, topicList);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingData;
using VotingWeb.DataContext;
using VotingWeb.Models.Votings;

namespace VotingWeb.Controllers
{
    public class TopicController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Topic
        public ActionResult Index(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var topic = TopicContext.Instance.FindById(entities, id);
                    var votingList = VotingContext.Instance.LoadList(entities, id);
                    var model = new VotingListModel(topic, votingList);
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
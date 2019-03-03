using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingData;
using VotingWeb.DataContext;
using VotingWeb.Models.Municipalities;

namespace VotingWeb.Controllers
{
    public class MunicipalityController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Municipality
        public ActionResult Index(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var municipality = MunicipalityContext.Instance.FindById(entities, id);
                    var sessionList = SessionContext.Instance.LoadList(entities, id);
                    var model = new SessionListModel(municipality, sessionList);
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
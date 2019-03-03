using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingData;
using VotingWeb.DataContext;
using VotingWeb.Models.Home;

namespace VotingWeb.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index(string hash)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    if (!string.IsNullOrWhiteSpace(hash))
                    {
                        var shortcut = ShortcutContext.Instance.FindByHash(entities, hash);
                        if (shortcut != null)
                        {
                            return RedirectToAction("Index", "Topic", new { id = shortcut.TopicId });
                        }
                    }
                    var municipalityList = MunicipalityContext.Instance.LoadList(entities);
                    var model = new MunicipalityListModel(municipalityList);
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
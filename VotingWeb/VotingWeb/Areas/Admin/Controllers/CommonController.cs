using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TableGenerator;
using TableGenerator.Models;
using VotingData;

namespace VotingWeb.Areas.Admin.Controllers
{
    public class CommonController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Admin/Common
        [ChildActionOnly]
        public ActionResult Table(TableModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    Generator.Instance.FillTable(entities.Database.Connection.ConnectionString, model);
                    return PartialView(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return new HttpStatusCodeResult(500);
            }
        }
    }
}
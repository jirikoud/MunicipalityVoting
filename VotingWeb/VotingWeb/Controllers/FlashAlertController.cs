using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingWeb.Infrastructure;

namespace VotingWeb.Controllers
{
    public class FlashAlertController : Controller
    {
        [ChildActionOnly]
        public ActionResult Index()
        {
            var model = ContextUtils.Instance.ReadActionStateCookie(Request, Response);
            return PartialView(model);
        }
    }
}
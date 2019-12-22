using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingCoreWeb.Infrastructure;

namespace VotingCoreWeb.ViewComponents
{
    public class AlertViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AlertModel alert)
        {
            AlertModel model = alert;
            if (alert == null && TempData["alertType"] != null)
            {
                model = new AlertModel((AlertTypeEnum)TempData["alertType"], TempData["alertMessage"].ToString());
            }
            return View(model);
        }
    }
}

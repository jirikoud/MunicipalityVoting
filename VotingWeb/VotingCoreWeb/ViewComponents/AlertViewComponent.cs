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
        public IViewComponentResult Invoke(AlertModel alert, bool isGeneral = false)
        {
            AlertModel model = alert;
            if (isGeneral && TempData[ContextUtils.TEMPDATA_ALERT_TYPE] != null)
            {
                model = new AlertModel((AlertTypeEnum)TempData[ContextUtils.TEMPDATA_ALERT_TYPE], TempData[ContextUtils.TEMPDATA_ALERT_MESSAGE].ToString());
            }
            return View(model);
        }
    }
}

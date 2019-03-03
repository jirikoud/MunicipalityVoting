using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using VotingData;
using VotingWeb.DataContext;
using VotingWeb.Infrastructure;

namespace VotingWeb.Areas.Admin.Controllers
{
    public abstract class BaseController : Controller
    {
        protected int? CheckMunicipalityRight(VotingEntities entities)
        {
            int? claimId = null;
            var user = User as ClaimsPrincipal;
            var municipalityClaim = user.Claims.FirstOrDefault(item => item.Type == Constants.CLAIM_MUNICIPALITY);
            if (municipalityClaim != null)
            {
                claimId = int.Parse(municipalityClaim.Value);
            }
            if (claimId.HasValue)
            {
                var municipality = MunicipalityContext.Instance.FindById(entities, claimId.Value);
                return (municipality != null ? claimId : (int?)null);
            }
            return claimId;
        }
    }
}
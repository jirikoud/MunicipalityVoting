using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VotingCommon;
using VotingCoreData;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Infrastructure
{
    public class ContextUtils
    {
        public const string TEMPDATA_ALERT_TYPE = "alertType";
        public const string TEMPDATA_ALERT_MESSAGE = "alertMessage";

        private string EncodeForCookie(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        private string DecodeFromCookie(string base64)
        {
            var bytes = Convert.FromBase64String(base64);
            string text = Encoding.UTF8.GetString(bytes);
            return text;
        }

        public void CreateActionStateCookie(ITempDataDictionary tempData, AlertTypeEnum actionType, string message)
        {
            tempData[TEMPDATA_ALERT_TYPE] = actionType;
            tempData[TEMPDATA_ALERT_MESSAGE] = message;
        }


        public string ShortenString(string fullString, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(fullString) || fullString.Length <= maxLength)
            {
                return fullString;
            }
            return fullString.Substring(0, maxLength - 1) + "…";
        }

        private async Task<bool> CheckMunicipalityRightAsync(ClaimsPrincipal user, VotingDbContext dbContext, int municipalityId)
        {
            var municipalityClaim = user.Claims.FirstOrDefault(item => item.Type == Constants.CLAIM_MUNICIPALITY);
            if (municipalityClaim != null)
            {
                int claimId = int.Parse(municipalityClaim.Value);
                if (claimId == municipalityId)
                {
                    var municipality = await dbContext.FindMunicipalityByIdAsync(municipalityId);
                    return true;
                }
            }
            return false;
        }

        private async Task<int?> GetAllowedMunicipalityAsync(ClaimsPrincipal user, VotingDbContext dbContext)
        {
            var municipalityClaim = user.Claims.FirstOrDefault(item => item.Type == Constants.CLAIM_MUNICIPALITY);
            if (municipalityClaim == null)
            {
                return null;
            }
            int claimId = int.Parse(municipalityClaim.Value);
            var municipality = await dbContext.FindMunicipalityByIdAsync(claimId);
            return municipality?.Id;
        }

        public async Task<int?> CheckMunicipalityRightsAsync(int? id, ClaimsPrincipal user, VotingDbContext dbContext, ITempDataDictionary tempData)
        {
            if (id.HasValue)
            {
                if (user.IsInRole(Constants.ROLE_ADMIN))
                {
                    var municipality = await dbContext.FindMunicipalityByIdAsync(id.Value);
                    if (municipality == null)
                    {
                        CreateActionStateCookie(tempData, AlertTypeEnum.Danger, AdminRes.ERROR_NOT_EXIST);
                        return null;
                    }
                }
                else
                {
                    var hasClaim = await CheckMunicipalityRightAsync(user, dbContext, id.Value);
                    if (!hasClaim)
                    {
                        CreateActionStateCookie(tempData, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return null;
                    }
                }
                return id.Value;
            }
            else
            {
                var claimId = await GetAllowedMunicipalityAsync(user, dbContext);
                if (claimId == null)
                {
                    CreateActionStateCookie(tempData, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                    return null;
                }
                return claimId.Value;
            }
        }
    }
}
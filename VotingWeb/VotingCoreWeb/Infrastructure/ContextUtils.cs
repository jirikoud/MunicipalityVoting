using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
                    var municipality = await dbContext.GetMunicipalityByIdAsync(municipalityId);
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
            var municipality = await dbContext.GetMunicipalityByIdAsync(claimId);
            return municipality?.Id;
        }

        public async Task<int?> CheckMunicipalityRightsAsync(int? id, ClaimsPrincipal user, VotingDbContext dbContext, ITempDataDictionary tempData)
        {
            if (id.HasValue)
            {
                if (user.IsInRole(Constants.ROLE_ADMIN))
                {
                    var municipality = await dbContext.GetMunicipalityByIdAsync(id.Value);
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

        public async Task<bool> CheckMunicipalityRightAsync(int municipalityId, IdentityUser user, VotingDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            var municipality = await dbContext.GetMunicipalityByIdAsync(municipalityId);
            if (municipality == null)
            {
                return false;
            }
            var isAdmin = await userManager.IsInRoleAsync(user, Constants.ROLE_ADMIN);
            if (isAdmin)
            {
                return true;
            }
            var claims = await userManager.GetClaimsAsync(user);
            if (claims.Any(item => item.Type == Constants.CLAIM_MUNICIPALITY && int.Parse(item.Value) == municipalityId))
            {
                return true;
            }
            return false;
        }

        private string GetApiKey(HttpRequest request)
        {
            var authorize = request.Headers["Authorization"].FirstOrDefault();
            if (authorize != null)
            {
                if (authorize.StartsWith("Bearer "))
                {
                    return authorize.Substring("Bearer ".Length);
                }
            }
            return null;
        }

        public async Task<IdentityUser> GetApiUserAsync(HttpRequest request, VotingDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            var apiKey = GetApiKey(request);
            var userId = await dbContext.GetUserIdByApiKeyAsync(apiKey);
            if (userId == null)
            {
                return null;
            }
            var user = await userManager.FindByIdAsync(userId);
            return user;
        }

        public string GetErrorMessage(Exception exception, string environment)
        {
            if (environment == Constants.ENVIRONMENT_DEV)
            {
                //Na vývoji posílat kompletní popis
                return exception.ToString();
            }
            return exception.Message;
        }

        public string GetErrorMessage(string message, string environment)
        {
            if (environment == Constants.ENVIRONMENT_DEV)
            {
                //Na vývoji posílat kompletní popis
                return message;
            }
            return null;
        }

    }
}
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using VotingWeb.Models;

namespace VotingWeb.Infrastructure
{
    public class ContextUtils
    {
        public const string ALERT_STATE_COOKIE = "alert-state";
        public const string COOKIE_VALUE_ACTION_TYPE = "type";
        public const string COOKIE_VALUE_MESSAGE = "message";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region --- Singleton ---

        private static ContextUtils instance;

        public static ContextUtils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ContextUtils();
                }
                return instance;
            }
        }

        #endregion

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

        public void CreateActionStateCookie(HttpResponseBase response, AlertTypeEnum actionType, string message)
        {
            var cookie = new HttpCookie(ALERT_STATE_COOKIE);
            cookie.Values.Add(COOKIE_VALUE_ACTION_TYPE, actionType.ToString());
            cookie.Values.Add(COOKIE_VALUE_MESSAGE, EncodeForCookie(message));
            response.Cookies.Add(cookie);
        }

        public AlertModel ReadActionStateCookie(HttpRequestBase request, HttpResponseBase response)
        {
            if (request.Cookies.AllKeys.Contains(ALERT_STATE_COOKIE))
            {
                var cookie = request.Cookies[ALERT_STATE_COOKIE];
                cookie.Expires = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                response.Cookies.Add(cookie);
                if (Enum.TryParse(cookie.Values[COOKIE_VALUE_ACTION_TYPE], out AlertTypeEnum actionType))
                {
                    return new AlertModel()
                    {
                        Class = AlertTypeConvertor.GetClass(actionType),
                        Title = AlertTypeConvertor.GetTitle(actionType),
                        Message = DecodeFromCookie(cookie.Values[COOKIE_VALUE_MESSAGE])
                    };
                }
            }
            return null;
        }

        public string ShortenString(string fullString, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(fullString) || fullString.Length <= maxLength)
            {
                return fullString;
            }
            return fullString.Substring(0, maxLength - 1) + "…";
        }
    }
}
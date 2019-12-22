using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Infrastructure
{
    public enum AlertTypeEnum
    {
        Info,
        Success,
        Warning,
        Danger,
    }

    public class AlertTypeConvertor
    {
        public static string GetClass(AlertTypeEnum actionType)
        {
            switch (actionType)
            {
                case AlertTypeEnum.Info:
                    return "alert-info";
                case AlertTypeEnum.Success:
                    return "alert-success";
                case AlertTypeEnum.Warning:
                    return "alert-warning";
                case AlertTypeEnum.Danger:
                    return "alert-danger";
            }
            return "alert-danger";
        }

        public static string GetTitle(AlertTypeEnum actionType)
        {
            switch (actionType)
            {
                case AlertTypeEnum.Info:
                    return CommonRes.ALERT_INFO;
                case AlertTypeEnum.Success:
                    return CommonRes.ALERT_SUCCESS;
                case AlertTypeEnum.Warning:
                    return CommonRes.ALERT_WARNING;
                case AlertTypeEnum.Danger:
                    return CommonRes.ALERT_DANGER;
            }
            return CommonRes.ALERT_DANGER;
        }
    }
}
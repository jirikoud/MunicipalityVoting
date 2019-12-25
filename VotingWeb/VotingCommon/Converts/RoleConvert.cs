using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using VotingCommon.Properties;

namespace VotingCommon.Converts
{
    public class RoleConvert
    {
        public static List<Tuple<string, string>> GetRoleList()
        {
            return new List<Tuple<string, string>>() {
                new Tuple<string, string>(Constants.ROLE_EDITOR, Resources.ROLE_EDITOR),
                new Tuple<string, string>(Constants.ROLE_ADMIN, Resources.ROLE_ADMIN),
            };
        }

        public static string Decode(string role)
        {
            if (role == Constants.ROLE_ADMIN)
            {
                return Resources.ROLE_ADMIN;
            }
            if (role == Constants.ROLE_EDITOR)
            {
                return Resources.ROLE_EDITOR;
            }
            return null;
        }
    }
}
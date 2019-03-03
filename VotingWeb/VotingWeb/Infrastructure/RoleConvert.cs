using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using VotingWeb.Properties;

namespace VotingWeb.Infrastructure
{
    public class RoleConvert
    {
        public static List<Tuple<string, string>> GetRoleList()
        {
            return new List<Tuple<string, string>>() {
                new Tuple<string, string>(Constants.ROLE_EDITOR, AdminRes.ROLE_EDITOR),
                new Tuple<string, string>(Constants.ROLE_ADMIN, AdminRes.ROLE_ADMIN),
            };
        }

        public static string Decode(string roleString)
        {
            if (string.IsNullOrWhiteSpace(roleString))
            {
                return null;
            }
            var builder = new StringBuilder();
            var split = roleString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in split)
            {
                if (role == Constants.ROLE_ADMIN)
                {
                    builder.Append(AdminRes.ROLE_ADMIN).Append(", ");
                }
                if (role == Constants.ROLE_EDITOR)
                {
                    builder.Append(AdminRes.ROLE_EDITOR).Append(", ");
                }
            }
            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 2, 2);
            }
            return builder.ToString();
        }
    }
}
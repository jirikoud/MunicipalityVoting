using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using VotingData;
using VotingWeb.DataContext;

namespace VotingWeb.Infrastructure
{
    public class ClaimConvert
    {
        public static string Decode(string claimString)
        {
            using (var entities = new VotingEntities())
            {
                if (string.IsNullOrWhiteSpace(claimString))
                {
                    return null;
                }
                var builder = new StringBuilder();
                var split = claimString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var claim in split)
                {
                    var intValue = int.Parse(claim);
                    var municipality = MunicipalityContext.Instance.FindById(entities, intValue);
                    if (municipality != null)
                    {
                        builder.Append(municipality.Name).Append(", ");
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
}
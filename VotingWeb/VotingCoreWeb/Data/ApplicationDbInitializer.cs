using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingCoreWeb.Data
{
    public class ApplicationDbInitializer
    {
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.FindByNameAsync("admin").Result == null)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "admin",
                    NormalizedName = "admin".ToUpper(),
                };
                var result = roleManager.CreateAsync(role).Result;
            }
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByEmailAsync("jiri_koudelka@yahoo.co.uk").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "jiri_koudelka@yahoo.co.uk",
                    Email = "jiri_koudelka@yahoo.co.uk"
                };
                var result = userManager.CreateAsync(user).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "admin").Wait();
                }
            }
        }
    }
}

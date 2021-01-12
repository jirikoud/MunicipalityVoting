using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace VotingCoreWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly ILogger<ApplicationDbContext> _logger;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        public async Task<List<IdentityUser>> LoadUsersAsync()
        {
            try
            {
                var list = await this.Users.OrderBy(item => item.UserName).ToListAsync();
                return list;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"LoadUsers()");
                return null;
            }
        }

        public async Task<List<IdentityRole>> LoadRolesAsync()
        {
            try
            {
                var list = await this.Roles.ToListAsync();
                return list;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"LoadRoles()");
                return null;
            }
        }

        public async Task<List<IdentityUserClaim<string>>> LoadClaimsAsync()
        {
            try
            {
                var list = await this.UserClaims.ToListAsync();
                return list;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"LoadClaims()");
                return null;
            }
        }
    }
}

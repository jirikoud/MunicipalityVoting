using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingCoreData.Models;

namespace VotingCoreData
{
    public class VotingDbContext : DbContext
    {
        private readonly ILogger<VotingDbContext> _logger;

        public VotingDbContext(DbContextOptions<VotingDbContext> options, ILogger<VotingDbContext> logger) : base(options)
        {
            _logger = logger;
        }

        public DbSet<Deputy> Deputies { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Shortcut> Shortcuts { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Voting> Votings { get; set; }

        #region Shortcut

        public async Task<Shortcut> FindShortcutByHashAsync(string hash)
        {
            try
            {
                var entity = await this.Shortcuts.FirstOrDefaultAsync(item => item.Hash == hash && !item.Topic.IsDeleted);
                return entity;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"FindShortcutByHash({hash})");
                return null;
            }
        }

        #endregion

        #region Municipality

        public async Task<List<Municipality>> LoadMunicipalitiesAsync()
        {
            try
            {
                var list = await this.Municipalities.Where(item => !item.IsDeleted).OrderBy(item => item.Name).ToListAsync();
                return list;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"LoadMunicipalities()");
                return null;
            }
        }

        public async Task<Municipality> FindMunicipalityById(int id)
        {
            try
            {
                var municipality = await this.Municipalities.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                return municipality;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"FindMunicipalityById({id})");
                return null;
            }
        }

        #endregion

        #region Session

        public async Task<List<Session>> LoadSessionsAsync(int municipalityId)
        {
            try
            {
                var list = await this.Sessions.Where(item => !item.IsDeleted && item.MunicipalityId == municipalityId).OrderByDescending(item => item.StartDate).ToListAsync();
                return list;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"LoadSessions({municipalityId})");
                return null;
            }
        }

        #endregion

    }
}

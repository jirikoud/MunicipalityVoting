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

        public async Task<bool> UpdateAsync()
        {
            try
            {
                await SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Update()");
                return false;
            }
        }

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

        public async Task<Municipality> FindMunicipalityByIdAsync(int id)
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

        public async Task<int?> CreateMunicipalityAsync(Municipality municipality)
        {
            try
            {
                this.Municipalities.Add(municipality);
                await SaveChangesAsync();
                return municipality.Id;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"CreateMunicipality()");
                return null;
            }
        }

        public async Task<bool> DeleteMunicipalityAsync(int id)
        {
            try
            {
                var municipality = await this.Municipalities.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                if (municipality != null)
                {
                    municipality.IsDeleted = true;
                }
                await SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"DeleteMunicipality({id})");
                return false;
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

        public async Task<Session> FindSessionByIdAsync(int id)
        {
            try
            {
                var municipality = await this.Sessions.Include(item => item.Municipality).FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                return municipality;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"FindSessionById({id})");
                return null;
            }
        }

        #endregion

        #region Topic

        public async Task<List<Topic>> LoadTopicsAsync(int sessionId)
        {
            try
            {
                var list = await this.Topics.Where(item => !item.IsDeleted && item.SessionId == sessionId).OrderBy(item => item.Order).ToListAsync();
                return list;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"LoadTopics({sessionId})");
                return null;
            }
        }

        public async Task<Topic> FindTopicByIdAsync(int id)
        {
            try
            {
                var municipality = await this.Topics.Include(item => item.Session.Municipality).FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                return municipality;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"FindTopicById({id})");
                return null;
            }
        }

        #endregion

        #region Voting

        public async Task<List<Voting>> LoadVotingsAsync(int topicId)
        {
            try
            {
                var list = await this.Votings.Include(item => item.Deputy).Include(item => item.Party).Where(item => item.TopicId == topicId).ToListAsync();
                return list;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"LoadVotings({topicId})");
                return null;
            }
        }

        #endregion

    }
}

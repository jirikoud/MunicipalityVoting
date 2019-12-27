using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingCommon.Models;
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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(user => new { user.RoleId, user.UserId});
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

        public async Task<int?> UpdateMunicipalityAsync(int? id, Municipality changes)
        {
            try
            {
                Municipality model;
                if (id.HasValue)
                {
                    model = await this.Municipalities.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                    if (model == null)
                    {
                        return null;
                    }
                    model.UpdateFrom(changes);
                }
                else
                {
                    model = new Municipality();
                    model.UpdateFrom(changes);
                    this.Municipalities.Add(model);
                }
                await SaveChangesAsync();
                return model.Id;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"UpdateMunicipality({id})");
                return null;
            }
        }

        public async Task<bool> DeleteMunicipalityAsync(int id)
        {
            try
            {
                var deleteItem = await this.Municipalities.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                if (deleteItem != null)
                {
                    deleteItem.IsDeleted = true;
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
                var session = await this.Sessions.Include(item => item.Municipality).FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                return session;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"FindSessionById({id})");
                return null;
            }
        }

        public async Task<int?> UpdateSessionAsync(int? id, Session changes)
        {
            try
            {
                Session model;
                if (id.HasValue)
                {
                    model = await this.Sessions.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                    if (model == null)
                    {
                        return null;
                    }
                    model.UpdateFrom(changes);
                }
                else
                {
                    model = new Session()
                    {
                        MunicipalityId = changes.MunicipalityId,
                    };
                    model.UpdateFrom(changes);
                    this.Sessions.Add(model);
                }
                await SaveChangesAsync();
                return model.Id;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"UpdateSession({id})");
                return null;
            }
        }

        public async Task<bool> DeleteSessionAsync(int id)
        {
            try
            {
                var deleteItem = await this.Sessions.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                if (deleteItem != null)
                {
                    deleteItem.IsDeleted = true;
                }
                await SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"DeleteSession({id})");
                return false;
            }
        }

        public async Task<int?> GetSessionTopicCountAsync(int id)
        {
            try
            {
                var count = await this.Topics.Where(item => !item.IsDeleted && item.SessionId == id).CountAsync();
                return count;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"GetSessionTopicCount({id})");
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

        public async Task<int?> UpdateTopicAsync(int? id, Topic changes)
        {
            try
            {
                Topic model;
                if (id.HasValue)
                {
                    model = await this.Topics.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                    if (model == null)
                    {
                        return null;
                    }
                    model.UpdateFrom(changes);
                }
                else
                {
                    model = new Topic()
                    {
                        SessionId = changes.SessionId,
                    };
                    model.UpdateFrom(changes);
                    this.Topics.Add(model);
                }
                await SaveChangesAsync();
                return model.Id;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"UpdateTopic({id})");
                return null;
            }
        }

        public async Task<bool> DeleteTopicAsync(int id)
        {
            try
            {
                var deleteItem = await this.Topics.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                if (deleteItem != null)
                {
                    deleteItem.IsDeleted = true;
                }
                await SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"DeleteTopic({id})");
                return false;
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

        public async Task<Voting> FindVotingByIdAsync(int id)
        {
            try
            {
                var party = await this.Votings.Include(item => item.Topic.Session).Include(item => item.Deputy).FirstOrDefaultAsync(item => item.Id == id);
                return party;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"FindVotingById({id})");
                return null;
            }
        }

        public async Task<int?> UpdateVotingAsync(int? id, Voting changes)
        {
            try
            {
                Voting model;
                if (id.HasValue)
                {
                    model = await this.Votings.FirstOrDefaultAsync(item => item.Id == id);
                    if (model == null)
                    {
                        return null;
                    }
                    model.UpdateFrom(changes);
                }
                else
                {
                    model = new Voting()
                    {
                        TopicId = changes.TopicId,
                    };
                    model.UpdateFrom(changes);
                    this.Votings.Add(model);
                }
                await SaveChangesAsync();
                return model.Id;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"UpdateVoting({id})");
                return null;
            }
        }

        public async Task<bool> DeleteVotingAsync(int id)
        {
            try
            {
                var deleteItem = await this.Votings.FirstOrDefaultAsync(item => item.Id == id);
                if (deleteItem != null)
                {
                    this.Votings.Remove(deleteItem);
                }
                await SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"DeleteVoting({id})");
                return false;
            }
        }

        #endregion

        #region Party

        public async Task<List<Party>> LoadPartiesAsync(int municipalityId)
        {
            try
            {
                var list = await this.Parties.Where(item => !item.IsDeleted && item.MunicipalityId == municipalityId).OrderBy(item => item.Name).ToListAsync();
                return list;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"LoadParties({municipalityId})");
                return null;
            }
        }

        public async Task<Party> FindPartyByIdAsync(int id)
        {
            try
            {
                var party = await this.Parties.Include(item => item.Municipality).FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                return party;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"FindPartyById({id})");
                return null;
            }
        }

        public async Task<int?> UpdatePartyAsync(int? id, Party changes)
        {
            try
            {
                Party model;
                if (id.HasValue)
                {
                    model = await this.Parties.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                    if (model == null)
                    {
                        return null;
                    }
                    model.UpdateFrom(changes);
                }
                else
                {
                    model = new Party()
                    {
                        MunicipalityId = changes.MunicipalityId,
                    };
                    model.UpdateFrom(changes);
                    this.Parties.Add(model);
                }
                await SaveChangesAsync();
                return model.Id;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"UpdateParty({id})");
                return null;
            }
        }

        public async Task<bool> DeletePartyAsync(int id)
        {
            try
            {
                var deleteItem = await this.Parties.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                if (deleteItem != null)
                {
                    deleteItem.IsDeleted = true;
                }
                await SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"DeleteParty({id})");
                return false;
            }
        }

        #endregion

        #region Deputy

        public async Task<List<Deputy>> LoadDeputiesAsync(int municipalityId)
        {
            try
            {
                var list = await this.Deputies.Where(item => !item.IsDeleted && item.MunicipalityId == municipalityId)
                    .OrderBy(item => item.Lastname)
                    .ThenBy(item => item.Firstname)
                    .ToListAsync();
                return list;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"LoadDeputies({municipalityId})");
                return null;
            }
        }

        public async Task<Deputy> FindDeputyByIdAsync(int id)
        {
            try
            {
                var party = await this.Deputies.Include(item => item.Municipality).FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                return party;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"FindDeputyById({id})");
                return null;
            }
        }

        public async Task<int?> UpdateDeputyAsync(int? id, Deputy changes)
        {
            try
            {
                Deputy model;
                if (id.HasValue)
                {
                    model = await this.Deputies.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                    if (model == null)
                    {
                        return null;
                    }
                    model.UpdateFrom(changes);
                }
                else
                {
                    model = new Deputy()
                    {
                        MunicipalityId = changes.MunicipalityId,
                    };
                    model.UpdateFrom(changes);
                    this.Deputies.Add(model);
                }
                await SaveChangesAsync();
                return model.Id;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"UpdateDeputy({id})");
                return null;
            }
        }

        public async Task<bool> DeleteDeputyAsync(int id)
        {
            try
            {
                var deleteItem = await this.Deputies.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                if (deleteItem != null)
                {
                    deleteItem.IsDeleted = true;
                }
                await SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"DeleteDeputy({id})");
                return false;
            }
        }

        #endregion

        #region User

        public async Task<List<User>> LoadUsersAsync()
        {
            try
            {
                var list = await this.Users
                    .Include(item => item.UserRoles).ThenInclude(item => item.Role)
                    .Include(item => item.Claims)
                    .OrderBy(item => item.UserName)
                    .ToListAsync();
                return list;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"LoadUsers()");
                return null;
            }
        }

        #endregion

        #region Import

        public async Task<bool> ImportSessionAsync(int municipalityId, SessionModel sessionModel)
        {
            try
            {
                var session = new Session()
                {
                    Name = sessionModel.Title,
                    Chairman = sessionModel.Chairman,
                    StartDate = sessionModel.StartDate,
                    EndDate = sessionModel.EndDate,
                    MunicipalityId = municipalityId,
                    Topics = new List<Topic>(),
                };
                this.Sessions.Add(session);

                var deputyDictionary = await this.Deputies.Where(item => item.MunicipalityId == municipalityId && !item.IsDeleted).ToDictionaryAsync(item => item.TitlePre + ";" + item.Firstname + ";" + item.Lastname + ";" + item.TitlePost, item => item);
                var partyDictionary = await this.Parties.Where(item => item.MunicipalityId == municipalityId && !item.IsDeleted).ToDictionaryAsync(item => item.Name, item => item);

                foreach (var topicModel in sessionModel.TopicList)
                {
                    var topic = new Topic()
                    {
                        Name = topicModel.Name,
                        Comment = topicModel.Comment,
                        Order = topicModel.Order,
                        Time = topicModel.Time,
                        Total = topicModel.DeputyTotal,
                        IsProcedural = topicModel.IsProcedural,
                        IsSecret = topicModel.IsSecret,
                    };
                    session.Topics.Add(topic);
                    foreach (var deputyModel in topicModel.DeputyList)
                    {
                        if (!partyDictionary.ContainsKey(deputyModel.Party))
                        {
                            var newParty = new Party()
                            {
                                MunicipalityId = municipalityId,
                                Name = deputyModel.Party,
                            };
                            this.Parties.Add(newParty);
                            partyDictionary.Add(newParty.Name, newParty);
                        }
                        var party = partyDictionary[deputyModel.Party];

                        var deputyKey = deputyModel.TitlePre + ";" + deputyModel.FirstName + ";" + deputyModel.Lastname + ";" + deputyModel.TitlePost;
                        if (!deputyDictionary.ContainsKey(deputyKey))
                        {
                            var newDeputy = new Deputy()
                            {
                                MunicipalityId = municipalityId,
                                Firstname = deputyModel.FirstName,
                                Lastname = deputyModel.Lastname,
                                TitlePre = deputyModel.TitlePre,
                                TitlePost = deputyModel.TitlePost,
                            };
                            this.Deputies.Add(newDeputy);
                            deputyDictionary.Add(deputyKey, newDeputy);
                        }
                        var deputy = deputyDictionary[deputyKey];

                        var voting = new Voting()
                        {
                            Deputy = deputy,
                            Party = party,
                            Topic = topic,
                            Vote = (int)deputyModel.Vote,
                        };
                        this.Votings.Add(voting);
                    }
                }
                await SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Import({municipalityId}) failed");
                return false;
            }
        }

        #endregion

    }
}

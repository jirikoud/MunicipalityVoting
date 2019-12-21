using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
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

    }
}

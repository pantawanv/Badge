using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Badge.Models;

namespace Badge.Data
{
    public class BadgeContext : DbContext
    {
        public BadgeContext (DbContextOptions<BadgeContext> options)
            : base(options)
        {
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupType> GroupTypes { get; set; }
        public DbSet<Leader> Leaders { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

    }
}

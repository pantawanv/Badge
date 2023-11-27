using Badge.Areas.Identity.Data;
using Badge.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Badge.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupType> GroupTypes { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
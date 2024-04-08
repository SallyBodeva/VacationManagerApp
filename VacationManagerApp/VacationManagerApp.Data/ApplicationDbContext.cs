using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacationManagerApp.Data.Models;

namespace VacationManagerApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(user => user.Team)
                .WithMany(team => team.Developers)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasOne(team => team.Leader)
                .WithOne(leader => leader.TeamLed)
                .HasForeignKey<User>(leader => leader.TeamLedId);

            modelBuilder.Entity<Team>()
                .HasOne(team => team.Project)
                .WithMany(project => project.Teams)
                .HasForeignKey(team => team.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VacationRequest>()
                .HasOne(a => a.Requester)
                .WithMany(r => r.VacationRequests);

            modelBuilder.Entity<User>()
                .Property(user => user.TeamId)
                .IsRequired(false);

            modelBuilder.Entity<User>()
               .Property(team => team.TeamLedId)
               .IsRequired(false);

            modelBuilder.Entity<Team>()
                .Property(team => team.ProjectId)
                .IsRequired(false);
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<VacationRequest> VacationRequests { get; set; }


    }
}

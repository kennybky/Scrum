using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Scrum.Data
{
    public class ScrumContext : IdentityDbContext<ScrumUser, ScrumRole, int>
    {

        public DbSet<ScrumTeam> ScrumTeams { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ScrumUserTeam> ScrumUserTeams { get; set; }

        public DbSet<ProductTeam> ProductTeams { get; set; }
        public DbSet<ProductBacklogItem> ProductBackLogItems { get; set; }

        public DbSet<BacklogTaskSchedule> BackLogTaskSchedules { get; set; }

        public DbSet<BacklogTask> BacklogTasks { get; set; }

        public DbSet<BacklogUpdate> BacklogUpdates { get; set; }

        public ScrumContext(DbContextOptions<ScrumContext> options)
            : base(options)
        {
           
        }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var PriorityConverter = new ValueConverter<Priority, string>(
            v => v.ToString(),
            v => (Priority)Enum.Parse(typeof(Priority), v));

            var ProductStatusConverter = new ValueConverter<ProductStatus, string>(
            v => v.ToString(),
            v => (ProductStatus)Enum.Parse(typeof(ProductStatus), v));

            var BacklogStatusConverter = new ValueConverter<BacklogStatus, string>(
            v => v.ToString(),
            v => (BacklogStatus)Enum.Parse(typeof(BacklogStatus), v));

            modelBuilder.Entity<ScrumUser>(b =>
            {
                b.ToTable("ScrumUsers");
               
            });

          

            

            modelBuilder.Entity<ScrumTeam>(b =>
            {
                b.ToTable("ScrumTeams");
                b.HasIndex(t => t.TeamName).IsUnique();
            });

            modelBuilder.Entity<Product>(b =>
            {
                b.HasIndex(pp => pp.Name).IsUnique();
                b.Property(p => p.ProductPriority).HasDefaultValue(Priority.NONE)
                .HasConversion(PriorityConverter);
                b.Property(p => p.ProductStatus).HasDefaultValue(ProductStatus.CONCEPTUALIZED)
                .HasConversion(ProductStatusConverter);
            });

            modelBuilder.Entity<ProductBacklogItem>(b =>
            {
                b.Property(i => i.Status).HasDefaultValue(BacklogStatus.CREATED)
                .HasConversion(BacklogStatusConverter);
                b.Property(i => i.Priority).HasDefaultValue(Priority.NONE)
                .HasConversion(PriorityConverter); ;
                b.Property(i => i.Created).HasDefaultValueSql("getutcdate()").IsRequired();
                b.Property(i => i.LastUpdated).HasDefaultValueSql("getutcdate()").IsRequired();
                
            });

            modelBuilder.Entity<BacklogTaskSchedule>(b =>
            {
                b.HasKey(s => new { s.BackLogTaskId, s.Day });
                b.HasOne(s => s.Task).WithMany(t => t.Schedule);
            });

            modelBuilder.Entity<BacklogUpdate>(b =>
            {
                b.HasIndex(u => u.UpdateTime);
                b.Property(u => u.UpdateTime).HasDefaultValueSql("getutcdate()").IsRequired();
              
            });

            modelBuilder.Entity<ScrumUserTeam>(b =>
            {
                b.HasKey(ut => new { ut.UserId, ut.TeamId });
                b.HasOne(ut => ut.User).WithMany(u => u.UserTeams);
                b.HasOne(ut => ut.Team).WithMany(t => t.UserTeams);
            });

            modelBuilder.Entity<ProductTeam>(b =>
            {
                b.HasKey(ut => new { ut.ProductId, ut.TeamId });
                b.HasOne(ut => ut.Product).WithMany(u => u.ProductTeams);
                b.HasOne(ut => ut.Team).WithMany(t => t.ProductTeams);
            });




            modelBuilder.Entity<IdentityUserClaim<int>>(b =>
            {
                b.ToTable("ScrumUserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<int>>(b =>
            {
                b.ToTable("ScrumUserLogins");
            });

            modelBuilder.Entity<IdentityUserToken<int>>(b =>
            {
                b.ToTable("ScrumUserTokens");
            });

            modelBuilder.Entity<ScrumRole>(b =>
            {
                b.ToTable("ScrumRoles");
            });

            modelBuilder.Entity<IdentityRoleClaim<int>>(b =>
            {
                b.ToTable("ScrumRoleClaims");
            });

            modelBuilder.Entity<IdentityUserRole<int>>(b =>
            {
                b.ToTable("ScrumUserRoles");
            });

           
        }
    }
}

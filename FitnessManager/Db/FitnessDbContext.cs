using FitnessManager.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessManager.Db
{
    public class FitnessDbContext : DbContext
    {
        public FitnessDbContext(DbContextOptions options) : base(options) {}

        public DbSet<Coach> Coaches { get; set; }

        public DbSet<Hall> Halls { get; set; }

        public DbSet<Training> Trainings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Training>()
                .ToTable("Training");

            modelBuilder.Entity<Training>()
                .Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Training>()
                .HasOne(t => t.Coach)
                .WithMany(c => c.Trainings);

            modelBuilder.Entity<Training>()
                .HasOne(t => t.Hall)
                .WithMany(h => h.Trainings);

            modelBuilder.Entity<Training>()
                .HasIndex(t => t.StartTime);

            modelBuilder.Entity<Coach>()
                .Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Coach>()
                .Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Coach>()
                .Property(c => c.Specialty)
                .HasConversion<int>();

            modelBuilder.Entity<Hall>()
                .Property(h => h.Title)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}

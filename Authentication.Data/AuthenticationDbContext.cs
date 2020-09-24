using System.Collections.Generic;
using Authentication.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Data
{
    public class AuthenticationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AuthenticationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedData(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.RefreshToken)
                .WithOne(rt => rt.User)
                .HasForeignKey<RefreshToken>(rt => rt.UserId);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.UserId)
                .IsUnique();

            modelBuilder.Entity<RolePermission>()
                .Property(rp => rp.PermissionType)
                .HasConversion<int>();

            modelBuilder.Entity<RolePermission>()
                .HasIndex(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasIndex(rp => new {rp.RoleId, rp.PermissionType})
                .IsUnique();
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var permissions = new List<RolePermission>
            {
                // Client
                new RolePermission { Id = 1, RoleId = 1, PermissionType = PermissionType.EditOwnProfile },
                new RolePermission { Id = 2, RoleId = 1, PermissionType = PermissionType.AssignTrainingToSelf },

                // Coach
                new RolePermission { Id = 3, RoleId = 2, PermissionType = PermissionType.EditOwnProfile },

                // Manager
                new RolePermission { Id = 4, RoleId = 3, PermissionType = PermissionType.EditOwnProfile },
                new RolePermission { Id = 5, RoleId = 3, PermissionType = PermissionType.AddCoaches},
                new RolePermission { Id = 6, RoleId = 3, PermissionType = PermissionType.AddHalls},
                new RolePermission { Id = 7, RoleId = 3, PermissionType = PermissionType.AddTrainings},
                new RolePermission { Id = 8, RoleId = 3, PermissionType = PermissionType.AddClients},

                // Chief
                new RolePermission { Id = 9, RoleId = 4, PermissionType = PermissionType.EditOwnProfile },
                new RolePermission { Id = 10, RoleId = 4, PermissionType = PermissionType.AddCoaches },
                new RolePermission { Id = 11, RoleId = 4, PermissionType = PermissionType.AddHalls },
                new RolePermission { Id = 12, RoleId = 4, PermissionType = PermissionType.AddTrainings },
                new RolePermission { Id = 13, RoleId = 4, PermissionType = PermissionType.AddClients },
                new RolePermission { Id = 14, RoleId = 4, PermissionType = PermissionType.ManageCoaches },
                new RolePermission { Id = 15, RoleId = 4, PermissionType = PermissionType.ManageHalls },
                new RolePermission { Id = 16, RoleId = 4, PermissionType = PermissionType.ManageTrainings },
                new RolePermission { Id = 17, RoleId = 4, PermissionType = PermissionType.ManageClients },
                new RolePermission { Id = 18, RoleId = 4, PermissionType = PermissionType.AddManagers },
                new RolePermission { Id = 19, RoleId = 4, PermissionType = PermissionType.ManageManagers },
            };

            modelBuilder.Entity<RolePermission>()
                .HasData(permissions);

            modelBuilder.Entity<Role>()
                .HasData(new[]
                {
                    new Role { Id = 1, Name = "Client" },
                    new Role { Id = 2, Name = "Coach" },
                    new Role { Id = 3, Name = "Manager" },
                    new Role { Id = 4, Name = "Chief" },
                });

            modelBuilder.Entity<User>()
                .HasData(new[]
                {
                    new User { Id = 1, RoleId = 1, Password = "123", Username = "1" },
                    new User { Id = 2, RoleId = 1, Password = "123", Username = "2" },
                    new User { Id = 3, RoleId = 1, Password = "123", Username = "3" },
                    new User { Id = 4, RoleId = 2, Password = "123", Username = "4" },
                    new User { Id = 5, RoleId = 3, Password = "123", Username = "5" },
                    new User { Id = 6, RoleId = 4, Password = "123", Username = "6" },
                });
        }
    }
}

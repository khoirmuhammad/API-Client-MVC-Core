using Microsoft.EntityFrameworkCore;
using VoxTestApplication.Models;
using VoxTestApplication.Models.User;
using VoxTestApplication.Models.Auth;
using VoxTestApplication.Models.Organizer;

namespace VoxTestApplication.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationLog>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ApplicationLog>().Property(p => p.ErrorMessage).HasMaxLength(250);
            modelBuilder.Entity<ApplicationLog>().Property(p => p.Level).HasMaxLength(20);
            modelBuilder.Entity<ApplicationLog>().Property(p => p.User).HasMaxLength(100);
        }

        public DbSet<ApplicationLog> ApplicationLogs { get; set; } = default!;

        public DbSet<VoxTestApplication.Models.User.UserUpdateRequest> UserRequestUpdate { get; set; }

        public DbSet<VoxTestApplication.Models.User.UserChangePasswordRequest> UserChangePassword { get; set; }

        public DbSet<VoxTestApplication.Models.Auth.RegisterRequest> RegisterRequest { get; set; }

        public DbSet<VoxTestApplication.Models.Organizer.Organizer> Organizer { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Area> Area { get; set; }
        public DbSet<BusinessGroup> BusinessGroup { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Rl_RolePermission> Rl_RolePermission { get; set; }
        public DbSet<Rl_UserArea> Rl_UserArea { get; set; }
        public DbSet<Rl_UserCity> Rl_UserCity { get; set; }
        public DbSet<Rl_UserRole> Rl_UserRole { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<ExamType> ExamType { get; set; }
        public DbSet<Exam> Exam { get; set; }
        public DbSet<ExamSubject> ExamSubject { get; set; }
        public DbSet<ExamResult> ExamResult { get; set; }
        public DbSet<CompanyLicense> CompanyLicense { get; set; }
        public DbSet<BusinessGroupLicense> BusinessGroupLicense { get; set; }
        public DbSet<AreaLicense> AreaLicense { get; set; }
        public DbSet<CityLicense> CityLicense { get; set; }
        public DbSet<LicenseLog> LicenseLog { get; set; }
        public DbSet<LicenseConfiguration> LicenseConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Subject>()
            //    .HasKey(s => s.Identifier);

            modelBuilder.Entity<Rl_UserRole>()
                .HasKey(ur => new { ur.UserID, ur.RoleID });

            modelBuilder.Entity<Rl_UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserID);

            modelBuilder.Entity<Rl_UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.RoleID);

            //
            modelBuilder.Entity<Rl_UserCity>()
                .HasKey(ur => new { ur.UserID, ur.CityID });

            modelBuilder.Entity<Rl_UserCity>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserCities)
                .HasForeignKey(ur => ur.UserID);

            modelBuilder.Entity<Rl_UserCity>()
                .HasOne(ur => ur.City)
                .WithMany(u => u.UserCities)
                .HasForeignKey(ur => ur.CityID);

            //
            modelBuilder.Entity<Rl_UserArea>()
                .HasKey(ur => new { ur.UserID, ur.AreaID });

            modelBuilder.Entity<Rl_UserArea>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserAreas)
                .HasForeignKey(ur => ur.UserID);

            modelBuilder.Entity<Rl_UserArea>()
                .HasOne(ur => ur.Area)
                .WithMany(u => u.UserAreas)
                .HasForeignKey(ur => ur.AreaID);

            //
            modelBuilder.Entity<Rl_RolePermission>()
                .HasKey(ur => new { ur.RoleID, ur.PermissionID });

            modelBuilder.Entity<Rl_RolePermission>()
                .HasOne(ur => ur.Role)
                .WithMany(u => u.RolePermissions)
                .HasForeignKey(ur => ur.RoleID);

            modelBuilder.Entity<Rl_RolePermission>()
                .HasOne(ur => ur.Permission)
                .WithMany(u => u.RolePermissions)
                .HasForeignKey(ur => ur.PermissionID);

            // Configuración de LicenseConfiguration
            modelBuilder.Entity<LicenseConfiguration>()
                .ToTable("LicenseConfiguration"); // Nombre exacto de la tabla en DB

        }
    }
}

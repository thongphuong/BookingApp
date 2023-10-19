using UserService.Domain;
using Microsoft.EntityFrameworkCore;
using UserService.Domain;
using UserService.Service.DTO.Return;

namespace UserService.Infrastructure
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var t = modelBuilder.Entity<ReturnDTO>().HasNoKey();

            //to support anonymous types, configure entity properties for read-only properties         
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Return> Return { get; set; }
        public DbSet<ReturnDetail> ReturnDetail { get; set; }

        public DbSet<RoleDetail> RoleDetails { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuAttribute> MenuAttributes { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Divisions> Division { get; set; }
        public DbSet<TimeFrame> TimeFrame { get; set; }
        public DbSet<SystemParameter> SystemParameter { get; set; }
    }
}

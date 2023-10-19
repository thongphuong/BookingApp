using BookingService.Domain;
using BookingService.Service;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<RoleDetail> RoleDetails { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Divisions> Division { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<OrderDelivery> OrderDeliveries { get; set; }
        public DbSet<OrderReceipt> OrderReceipts { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Return> Return { get; set; }
        public DbSet<ReturnDetail> ReturnDetail { get; set; }
        public DbSet<LineDepartment> LineDepartmentsLines { get; set; }
        public DbSet<SystemParameter> SystemParameters { get; set; }
        public DbSet<TimeFrame> TimeFrame { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("USER")
                .Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<Store>().ToTable("STORE")
                .Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<RoleDetail>().ToTable("ROLE_DETAIL")
                .Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<Department>().ToTable("DEPARTMENT")
                .Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<Divisions>().ToTable("DIVISIONS")
              .Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<SystemParameter>().ToTable("SYSTEM_PARAMETER")
                .Metadata.SetIsTableExcludedFromMigrations(true);
            modelBuilder.Entity<TimeFrame>().ToTable("TIME_FRAME")
               .Metadata.SetIsTableExcludedFromMigrations(true);
            var t = modelBuilder.Entity<ReturnDTO>().HasNoKey();
            //to support anonymous types, configure entity properties for read-only properties         
            base.OnModelCreating(modelBuilder);
        }
    }
}

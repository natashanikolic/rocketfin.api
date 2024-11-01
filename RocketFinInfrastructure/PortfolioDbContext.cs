using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RocketFinDomain.Entities;

namespace RocketFinInfrastructure
{
    public class PortfolioDbContext: DbContext
    {
        protected readonly IConfiguration Configuration;

        public PortfolioDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DbSet<Trade> Trades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trade>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Symbol).IsRequired().HasMaxLength(10);
                entity.Property(t => t.Quantity).IsRequired();
                entity.Property(t => t.PriceAtTransaction).HasColumnType("decimal(18,2)");
                entity.Property(t => t.TransactionDate).IsRequired();
                entity.Property(t => t.TradeType).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

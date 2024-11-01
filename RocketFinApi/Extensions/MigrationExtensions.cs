using Microsoft.EntityFrameworkCore;
using RocketFinInfrastructure;

namespace RocketFinApi.Extensions
{
    public static class MigrationExtensions
    {
        public static async Task ApplyMigrations(this IApplicationBuilder app)
        {
            await ApplyMigrations(app.ApplicationServices);
        }

        public static async Task ApplyMigrations(this IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            using PortfolioDbContext context = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();

            //context.Database.SetConnectionString("Server=localhost;Database=rocketfindb;Trusted_Connection=True;TrustServerCertificate=true;Application Name=rocketfinapi;");
            context.Database.Migrate();
            
        }
    }
}

using GraduationProject.Domain.Contracts;
using GraduationProject.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GraduationProject.API.Extensions
{
    public static class WebApplicationRegister
    {
        public static async Task<WebApplication> MigrationDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            var PendingMigration = await dbContext.Database.GetPendingMigrationsAsync();
            if (PendingMigration.Any())
            {
                dbContext.Database.Migrate();
            }
            return app;

        }
        public static async Task<WebApplication> SeedData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataSeed>();

            await dataInitializer.InitializeDataAsync();
            return app;

        }
    }
}

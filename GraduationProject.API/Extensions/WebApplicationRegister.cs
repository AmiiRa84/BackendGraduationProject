using GraduationProject.Domain.Contracts;
using GraduationProject.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace GraduationProject.API.Extensions
{
    public static class WebApplicationRegister
    {
        public static WebApplication MigrationDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
            return app;

        }
        public static WebApplication SeedData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var DataSeed = scope.ServiceProvider.GetRequiredService<IDataSeed>();//3awez objectr mn class by implement lidataseed

            DataSeed.InitializeData();
            return app;

        }
    }
}

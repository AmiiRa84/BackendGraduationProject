using GraduationProject.Domain.Contracts;
using GraduationProject.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Extensions
{
    public static class WebApplicationRegister
    {
     
        public static async Task<WebApplication> MigrationDataBaseAsync(this WebApplication app)
        {
          
            await using var scoop = app.Services.CreateAsyncScope();

            var dbcontext = scoop.ServiceProvider.GetRequiredService<StoreDbContext>();//object mn ldbcontext
            var PendingMigration = await dbcontext.Database.GetPendingMigrationsAsync();
            if (PendingMigration.Any())
            {
                dbcontext.Database.Migrate();//e3ml apply ;l ay pending migration
            }
            return app;
        }
        public static async Task<WebApplication> MigrationSecurityDataBaseAsync(this WebApplication app)
        {
            await using var scoop = app.Services.CreateAsyncScope();

            var dbcontext = scoop.ServiceProvider.GetRequiredService<StoreDbContext>();
            var PendingMigration = await dbcontext.Database.GetPendingMigrationsAsync();
            if (PendingMigration.Any())
            {
                dbcontext.Database.Migrate();//e3ml apply ;l ay pending migration
            }
            return app;
        }
     
        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            using var scoop = app.Services.CreateScope();


            var dataInitializer = scoop.ServiceProvider.GetRequiredKeyedService<IDataSeed>("Default");

           await dataInitializer.InitializeDataAsync();
            return app;

        }
        public static async Task<WebApplication> SeedIdentityDataAsync(this WebApplication app)
        {
            using var scoop = app.Services.CreateScope();


            var dataInitializer = scoop.ServiceProvider.GetRequiredKeyedService<IDataSeed>("Identity");//keda ana 3awezo yedene object mn l identitydatainitilizer


            await dataInitializer.InitializeDataAsync();
            return app;

        }
    }
}

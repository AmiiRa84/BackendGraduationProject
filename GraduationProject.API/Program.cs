using ECommerce.API.Extensions;
using ECommerce.Domain.Contracts;
using ECommerce.persistence.Repositories;
using GraduationProject.API.CustomMiddlewares;
using GraduationProject.Domain.Contracts;
using GraduationProject.Domain.Entities.SecurityModule;
using GraduationProject.Persistence.Data.DataSeed;
using GraduationProject.Persistence.Data.DbContexts;
using GraduationProject.Persistence.IdentityData.DataSeed;
using GraduationProject.Services;
using GraduationProject.Services.Abstraction;
using GraduationProject.Services.MappingProfiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Register with Dependency Injection Container

            builder.Services.AddControllers()
                .AddApplicationPart(typeof(GraduationProject.Presentation.Controllers.AuthController).Assembly);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<StoreDbContext>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddKeyedScoped<IDataSeed, IdentityDataInitializer>("Identity");
            builder.Services.AddKeyedScoped<IDataSeed, DataSeed>("Default");

            builder.Services.AddScoped<IDashboardService, DashboardService>();

            #region All Profiles
            builder.Services.AddAutoMapper(x => x.AddProfile(typeof(TaskProfile)));
            builder.Services.AddAutoMapper(x => x.AddProfile(typeof(SpecialistProfile)));
            builder.Services.AddAutoMapper(x => x.AddProfile(typeof(ChildProfile)));
            builder.Services.AddAutoMapper(x => x.AddProfile(typeof(ParentProfile)));
            #endregion

            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<IChildServices, ChildServices>();
            builder.Services.AddScoped<ISpecialistServices, SpecialistServices>();
            builder.Services.AddScoped<IParentService, ParentService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddHttpClient<ISendbirdService, SendBirdService>();
            builder.Services.AddScoped<ISendbirdSyncService, SendbirdSyncService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });
            builder.Services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 7115;
            });
           
            builder.Services.AddHttpClient<IAIReportService, AIReportService>(client =>
            {
                client.DefaultRequestHeaders.ExpectContinue = false;
            });

            // ? EmailService
            builder.Services.AddScoped<IEmailService, EmailService>();
            #endregion

            var app = builder.Build();

            await app.MigrationDataBaseAsync();
            await app.SeedIdentityDataAsync();
            await app.SeedDataAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

using ECommerce.Domain.Contracts;
using ECommerce.persistence.Repositories;
using GraduationProject.API.Extensions;
using GraduationProject.Domain.Contracts;
using GraduationProject.Persistence.Data.DataSeed;
using GraduationProject.Persistence.Data.DbContexts;
using GraduationProject.Services;
using GraduationProject.Services.Abstraction;
using GraduationProject.Services.MappingProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace GraduationProject.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Register with Dependency Injection Container
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IDataSeed, DataSeed>();
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
            builder.Services.AddScoped<IParentService   , ParentService>();
            builder.Services.AddHttpClient<ISendbirdService, SendBirdService>();
           
            #endregion

            var app = builder.Build();

            await app.MigrationDatabase();
            await app.SeedData();

            #region Configure Pipeline 
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}

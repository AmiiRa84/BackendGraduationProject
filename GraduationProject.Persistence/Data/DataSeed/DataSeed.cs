using GraduationProject.Domain.Contracts;
using GraduationProject.Domain.Entities;
using GraduationProject.Domain.Entities.TaskModule;
using GraduationProject.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GraduationProject.Persistence.Data.DataSeed
{
    public class DataSeed : IDataSeed
    {
        private readonly StoreDbContext _dbContexts;

        public DataSeed(StoreDbContext dbContexts)
        {
            _dbContexts = dbContexts;
        }

        public async Task InitializeDataAsync()
        {
           try
            {
                var hasPredefinedTasks =await _dbContexts.PreDefinedTasks.AnyAsync();
                if (hasPredefinedTasks) //lw l table bta3 l tasks msh fadi
                {
                    return;
                }
                if(!hasPredefinedTasks)
                {
                   await SeedFromJson<PreDefinedTask, int>("PreDefinedTasks.json", _dbContexts.PreDefinedTasks);


                }

                await  _dbContexts.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Cannot initialize predefined task : {ex}");
            }
            
        }
        private async Task SeedFromJson<T,Tkey>(string fileName,DbSet<T> dbset) where T: BaseEntity<Tkey>
        {
            //A:\Backend\Backend Graduation Project\BackendGraduationProject\GraduationProject\GraduationProject.Persistence\Data\DataSeed\JsonFiles\PreDefinedTasks.json
            var filePath = @"A:\FinalGraduationProject\GraduationProject.Persistence\Data\DataSeed\JsonFiles\" + fileName;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Json file not Found ", filePath);
            }
            try
            {
                var dataStream = File.OpenRead(filePath);

              
                var FileData =await JsonSerializer.DeserializeAsync<List<T>>(dataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
                });

                if (FileData is not null)
                {
                    //Console.WriteLine($"Found {FileData?.Count ?? 0} items to seed from {filePath}");
                  await  dbset.AddRangeAsync(FileData);
                
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"SeedFrom json failed : {ex}");
            }

        }

        
    }
}

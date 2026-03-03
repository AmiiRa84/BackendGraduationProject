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

        public void InitializeData()
        {
           try
            {
                if (_dbContexts.preDefinedTasks.Any()) //lw l table bta3 l tasks msh fadi
                {
                    return;
                }
                if(!_dbContexts.preDefinedTasks.Any())
                SeedFromJson<PreDefinedTask, int>("PreDefinedTasks.json", _dbContexts.preDefinedTasks);

                _dbContexts.SaveChanges();

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Cannot initialize predefined task : {ex}");
            }
            
        }
        private void SeedFromJson<T,Tkey>(string fileName,DbSet<T> dbset) where T: BaseEntity<Tkey>
        {
    var filePath = @"A:\BackendGraduationProject\GraduationProject\GraduationProject.Persistence\Data\DataSeed\JsonFiles\" +fileName;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Json file not Found ", filePath);
            }
            try
            {
                var dataStream = File.OpenRead(filePath);

              
                var FileData = JsonSerializer.Deserialize<List<T>>(dataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
                });

                if (FileData is not null)
                {
                    Console.WriteLine($"Found {FileData?.Count ?? 0} items to seed from {filePath}");
                    dbset.AddRange(FileData);
                
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"SeedFrom json failed : {ex}");
            }

        }

        
    }
}

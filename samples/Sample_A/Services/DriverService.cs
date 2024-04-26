
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Sample_A.Configurations;
using Sample_A.Models;

namespace Sample_A.Services
{
    public class DriverService
    {
        private readonly IMongoCollection<Driver> _driverCollection;

        public DriverService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _driverCollection = mongoDb.GetCollection<Driver>(databaseSettings.Value.CollectionName);
        }

        public async Task<List<Driver>> GetAsync()=> await _driverCollection.Find(_=>true).ToListAsync();

        public async Task<Driver> GetAsync(string id)=> await _driverCollection.Find(driver=>driver.Id==id).FirstOrDefaultAsync();

         public async Task CreateAsync(Driver driver) => await _driverCollection.InsertOneAsync(driver);

         public async Task UpdateAsync(Driver driver)=> await _driverCollection.ReplaceOneAsync(d=>d.Id==driver.Id, driver);

         public async Task RemoveAsync(string id)=> await _driverCollection.DeleteOneAsync(d=>d.Id==id);
    }
}
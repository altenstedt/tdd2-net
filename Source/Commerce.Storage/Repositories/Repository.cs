using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using Commerce.Storage.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Commerce.Storage.Repositories
{
    public class Repository : IRepository
    {
        private readonly IMongoDatabase database;

        public Repository(DatabaseOptions options) {
            var settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(options.Host, options.Port);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            var  identity = new MongoInternalIdentity(options.DatabaseName, options.Username);

            var evidence = new PasswordEvidence(options.Password);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            var client = new MongoClient(settings);

            database = client.GetDatabase(options.DatabaseName);
        }

        public async Task<BasketEntity> GetById(string id)
        {
            var collection = database.GetCollection<BasketEntity>("commerce");

            var cursor = await collection.FindAsync<BasketEntity>(new BsonDocument("_id", id));

            return await cursor.FirstAsync();
        }

        public async Task<BasketEntity> InsertOrUpdate(BasketEntity basket)
        {
            basket.Id = basket.Id ?? Guid.NewGuid().ToString();

            var collection = database.GetCollection<BasketEntity>("commerce");

            var filter = Builders<BasketEntity>.Filter.Eq("_id", basket.Id);
            await collection.ReplaceOneAsync(filter, basket, new UpdateOptions { IsUpsert = true });

            return basket;
        }
    }
}
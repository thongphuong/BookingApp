using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;

namespace UserService.Infrastructure
{
    public class MongoDbContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public MongoDbContext(IConfiguration _config)
        {
            try
            {
                var pass = "";
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGO_USER")))
                {
                    pass = $"{Environment.GetEnvironmentVariable("MONGO_USER")}:{Environment.GetEnvironmentVariable("MONGO_PASS")}@";
                }
                var connectStr = (_config["UseEnv"] ?? "0") == "0" ? _config["MongoConnection"] : $"mongodb://{pass}{Environment.GetEnvironmentVariable("MONGO_URL")}:{Environment.GetEnvironmentVariable("MONGO_PORT")}/";
                _mongoClient = new MongoClient(connectStr);
                _db = _mongoClient.GetDatabase(Environment.GetEnvironmentVariable("MONGO_DATABASE"));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public IMongoCollection<T> GetCollection<T>() where T : IMongo
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c._id)
                        .SetIdGenerator(new StringObjectIdGenerator())
                        .SetSerializer(new StringSerializer(BsonType.ObjectId));
                });
            }
            return _db.GetCollection<T>(typeof(T).Name);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

using Amazon.Lambda.Core;
using MongoDB.Bson.Serialization.Attributes;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ServerlessZipRetriever.Service
{
    public class ZipRetriever
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Handler(RequestData input, ILambdaContext context)
        {
            // Environment variables
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            var databaseName = Environment.GetEnvironmentVariable("DatabaseName");
            var collectionName = Environment.GetEnvironmentVariable("CollectionName");

            // Get database connection
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            var collectionUsaZips = database.GetCollection<Usazips>(collectionName);

            // Query data
            var city = input.City;
            var state = input.State;
            var filter = new BsonDocument(new Dictionary<string, object> {
                { "city", city},
                { "state", state},
            });
            var usazip = collectionUsaZips.Find(filter).FirstOrDefault();

            // Return
            return usazip.Id;
        }
    }

    public class RequestData
    {
        public string City { get; private set; }
        public string State { get; private set; }

        public RequestData(String city, String state )
        {
            City = city;
            State = state;
        }
    }

    [BsonIgnoreExtraElements]
    public class Usazips
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("state")]
        public string State { get; set; }
    }
}

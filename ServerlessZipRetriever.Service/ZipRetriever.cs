using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

using Amazon.Lambda.Core;
using ServerlessZipRetriever.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ServerlessZipRetriever
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
            var collection = database.GetCollection<Zip>(collectionName);

            // Query data
            var city = input.City;
            var state = input.State;
            var filter = new BsonDocument(new Dictionary<string, object> {
                { "city", city},
                { "state", state},
            });
            var usazip = collection.Find(filter).FirstOrDefault();

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
}

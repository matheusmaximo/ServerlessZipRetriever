using System;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

using ServerlessZipRetriever.Manager;
using ServerlessZipRetriever.Persistence;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ServerlessZipRetriever
{
    /// <summary>
    /// Class do retrieve zip code data from a MongoDB database
    /// </summary>
    public class ZipRetriever
    {
        /// <summary>
        /// Function to collect data from a MongoDB database and return zip code
        /// </summary>
        /// <param name="input">[<see cref="RequestData"/>]</param>
        /// <param name="context">[<see cref="ILambdaContext"/>]</param>
        /// <returns>Zip code found</returns>
        public async Task<string> Handler(RequestData input, ILambdaContext context)
        {
            // Get environment variables
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            var databaseName = Environment.GetEnvironmentVariable("DatabaseName");
            var collectionName = Environment.GetEnvironmentVariable("CollectionName");

            // Get db connection
            var connectionData = new ConnectionData(connectionString, databaseName, collectionName);
            var dbContext = new ZipCodeDbContext(connectionData);

            // Get manager
            var zipManager = new ZipCodeManager(dbContext);

            // Required data
            var state = input.State;
            var city = input.City;
            string result = await zipManager.GetZip(state, city);

            return result;
        }

        /// <summary>
        /// Helper class to hold required data to get zipcode information
        /// </summary>
        public class RequestData
        {
            public string City { get; }
            public string State { get; }

            public RequestData(String city, String state)
            {
                City = city;
                State = state;
            }
        }
    }
}

using MongoDB.Driver;
using ServerlessZipRetriever.Model;

namespace ServerlessZipRetriever.Persistence
{
    /// <summary>
    /// Class to hold db connection
    /// </summary>
    public class ZipCodeDbContext
    {
        /// <summary>
        /// Initializes a new instance of the ZipCodeDbContext class.
        /// </summary>
        /// <param name="connectionData">Required data do connect database</param>
        public ZipCodeDbContext(ConnectionData connectionData)
        {
            // Get database connection
            client = new MongoClient(connectionData.ConnectionString);
            database = client.GetDatabase(connectionData.DatabaseName);
            collectionName = connectionData.CollectionName;
        }
        
        /// <summary>
        /// ZipCode collection, see [<see cref="IMongoCollection<ZipCode>"/>]
        /// </summary>
        public IMongoCollection<ZipCode> ZipCodeCollection
        {
            get
            {
                return database.GetCollection<ZipCode>(collectionName);
            }
        }

        private readonly MongoClient client;
        private readonly IMongoDatabase database;
        private readonly string collectionName;
    }

    /// <summary>
    /// Helper class do get required data to connect MongoDB database
    /// </summary>
    public class ConnectionData
    {
        public string ConnectionString { get; }
        public string DatabaseName { get; }
        public string CollectionName { get; }

        /// <summary>
        /// Initializes a new instance of the ConnectionData class.
        /// </summary>
        /// <param name="connectionString">MongoDB connection string</param>
        /// <param name="databaseName">Database do connect</param>
        /// <param name="collectionName">Collection to get data from</param>
        public ConnectionData(string connectionString, string databaseName, string collectionName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            CollectionName = collectionName;
        }
    }
}

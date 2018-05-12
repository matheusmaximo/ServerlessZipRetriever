using MongoDB.Driver;
using ServerlessZipRetriever.Model;
using ServerlessZipRetriever.Persistence;
using System.Linq;
using System.Threading.Tasks;

namespace ServerlessZipRetriever.Manager
{
    /// <summary>
    /// Class to manager ZipCode data
    /// </summary>
    public class ZipCodeManager
    {
        private readonly ZipCodeDbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the ZipCodeManager class.
        /// </summary>
        /// <param name="dbContext">Database connection do get data</param>
        public ZipCodeManager(ZipCodeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Get zip code from city and state
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="city">City</param>
        /// <returns>If found, the required zip code</returns>
        public async Task<string> GetZip(string state, string city)
        {
            // Query data
            FindOptions<ZipCode> options = new FindOptions<ZipCode> { Limit = 1 };
            var task = await dbContext.ZipCodeCollection.FindAsync(e => e.City == city && e.State == state, options);
            var list = await task.ToListAsync();
            var zip = list.FirstOrDefault();

            return zip?.Id;
        }
    }
}

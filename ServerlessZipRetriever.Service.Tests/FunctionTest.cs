using Xunit;
using Amazon.Lambda.TestUtilities;
using System;

namespace ServerlessZipRetriever.Tests
{
    public class FunctionTest
    {
        [Theory]
        [InlineData("MA", "AGAWAM", "01001")]
        [InlineData("MA", "CUSHMAN", "01002")]
        [InlineData("NH", "MONT VERNON", "03057")]
        public void TestZipRetrieverFunction(string state, string city, string expected)
        {
            // Arrange
            // Invoke the lambda function and confirm the string was upper cased.
            var function = new ZipRetriever();
            var context = new TestLambdaContext();
            var request = new RequestData(city: city, state: state);
            Environment.SetEnvironmentVariable("ConnectionString", "mongodb://matheus:Passw0rd!@ds016118.mlab.com:16118/globalzips");
            Environment.SetEnvironmentVariable("DatabaseName", "globalzips");
            Environment.SetEnvironmentVariable("CollectionName", "usazips");

            // Act
            var result = function.Handler(request, context);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}

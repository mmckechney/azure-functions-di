using System;
using Xunit;

namespace Azure.Functions.DI.Tests
{
    public class HttpTriggerTest
    {
        [Fact]
        public void DummyHttpTest()
        {
            // Initial test to pass CI/CD pipeline.
            Assert.True(true);
        }
    }
}

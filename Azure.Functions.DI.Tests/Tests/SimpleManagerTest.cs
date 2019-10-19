using Azure.Functions.DI.Interfaces;
using Azure.Functions.DI.Tests.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Azure.Functions.DI.Tests
{
    [TestClass]
    public class SimpleManagerTest
    {
        private ServiceProvider services;
        private ISimpleManager simpleManager;

        [TestInitialize]
        public void Initialize()
        {
            services = UnitTestDIManager.ConfigureServices();
            simpleManager = services.GetService<ISimpleManager>();
        }

        [TestMethod]
        public void ToUpperCaseTest()
        {
            string name = "Ivy";
            string nameUpperCase = "IVY";

            // Names are not the same (case sensetive)
            Assert.AreNotEqual(name, nameUpperCase);

            name = simpleManager.ToUpperCase(name);

            // Names should be the same now
            Assert.AreEqual(name, nameUpperCase);
        }
    }
}

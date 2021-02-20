using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Functions.DI.Interfaces;
using Azure.Functions.DI.Tests.Configurations;
using Azure.Functions.DI.Tests.Managers;
using Azure.Functions.DI.Tests.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Azure.Functions.DI.Tests
{
    [TestClass]
    public class SimpleHttpFunctionTest
    {
        private ServiceProvider services;
        private MoqManager mockManager;
        private ISimpleManager simpleManager;
        private Person person;
        private Mock<HttpRequest> mockRequest;
        private Mock<ILogger> log;
        private Dictionary<string, StringValues> mockRequestParam;
        private SimpleHttpFunction simpleHttpFunction;
        
        [TestInitialize]
        public void Initialize()
        {
            services = UnitTestDIManager.ConfigureServices();
            mockManager = services.GetService<MoqManager>();
            simpleManager = services.GetService<ISimpleManager>();
            mockRequestParam = new Dictionary<string, StringValues>();
            simpleHttpFunction = new SimpleHttpFunction(simpleManager);
            person = new Person { Name = "Ivy" };
            log = new Mock<ILogger>();
        }

        [TestMethod]
        public async Task SimpleHttpFunctionBadRequestTest()
        {
            // Mock http request with empty body and empty query params
            mockRequest = mockManager.CreateMockRequest(mockRequestParam, null);
            
            var response = await simpleHttpFunction.Run(mockRequest.Object, log.Object);
            
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task SimpleHttpFunctionWithBodyTest()
        {
            // Mock http request with body and empty query params
            mockRequest = mockManager.CreateMockRequest(mockRequestParam, person);

            var response = await simpleHttpFunction.Run(mockRequest.Object, log.Object);
            var responsevalue = ((OkObjectResult)response).Value as string;

            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual($"Hello, {simpleManager.ToUpperCase(person.Name)}.", responsevalue);
        }

        [TestMethod]
        public async Task SimpleHttpFunctionWithQueryParamTest()
        {
            // Mock http request with empty body and query params
            mockRequestParam.Add("name", person.Name);
            mockRequest = mockManager.CreateMockRequest(mockRequestParam, null);
            
            var response = await simpleHttpFunction.Run(mockRequest.Object, log.Object);
            var responsevalue = ((OkObjectResult)response).Value as string;

            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual($"Hello, {simpleManager.ToUpperCase(person.Name)}.", responsevalue);
        }
    }
}

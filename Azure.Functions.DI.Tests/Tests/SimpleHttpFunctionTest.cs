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
        }

        [TestMethod]
        public async Task TestBadRequestResponse()
        {
            // Mock http request with empty body and no query params
            mockRequest = mockManager.CreateMockRequest(null);
            mockRequest.Setup(x => x.Query).Returns(new QueryCollection(mockRequestParam));

            var response = await simpleHttpFunction.Run(mockRequest.Object, new Mock<ILogger>().Object);
            
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task TestResponseWithBody()
        {
            // Mock http request with body and empty query params
            mockRequest = mockManager.CreateMockRequest(person);
            mockRequest.Setup(x => x.Query).Returns(new QueryCollection(mockRequestParam));

            var response = await simpleHttpFunction.Run(mockRequest.Object, new Mock<ILogger>().Object);
            var responsevalue = ((OkObjectResult)response).Value as string;

            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual($"Hello, {simpleManager.ToUpperCase(person.Name)}.", responsevalue);
        }

        [TestMethod]
        public async Task TestResponseQueryParam()
        {
            // Mock http request with empty body and query params
            mockRequest = mockManager.CreateMockRequest(null);
            mockRequestParam.Add("name", person.Name);
            mockRequest.Setup(x => x.Query).Returns(new QueryCollection(mockRequestParam));

            var response = await simpleHttpFunction.Run(mockRequest.Object, new Mock<ILogger>().Object);
            var responsevalue = ((OkObjectResult)response).Value as string;

            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual($"Hello, {simpleManager.ToUpperCase(person.Name)}.", responsevalue);
        }
    }
}

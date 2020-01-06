using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Azure.Functions.DI.Tests.Managers
{
    public class MoqManager
    {
        public Mock<HttpRequest> CreateMockRequest(Dictionary<string, StringValues> query, object body)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            var json = JsonConvert.SerializeObject(body);

            sw.Write(json);
            sw.Flush();

            ms.Position = 0;

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Query).Returns(new QueryCollection(query));
            mockRequest.Setup(x => x.Body).Returns(ms);
            
            return mockRequest;
        }
    }
}

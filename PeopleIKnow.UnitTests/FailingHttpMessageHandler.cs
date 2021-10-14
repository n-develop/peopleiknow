using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PeopleIKnow.UnitTests
{
    public class FailingHttpMessageHandler : MockHttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Calls++;
            throw new Exception("Request Failed");
        }
    }
}
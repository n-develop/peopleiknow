using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PeopleIKnow.UnitTests
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public int Calls { get; private set; } = 0;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Calls++;
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
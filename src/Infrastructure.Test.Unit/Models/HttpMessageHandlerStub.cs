using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Test.Unit.Models
{
    public class HttpMessageHandlerStub : HttpMessageHandler
    {
        public IEnumerable<HttpRequestMessage> Requests { get; set; } = Enumerable.Empty<HttpRequestMessage>();
        public IDictionary<string, HttpResponseMessage> Responses { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests = Requests.Append(request);

            return Responses == null || !Responses.Any() ?
                Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)) :
                Task.FromResult(Responses[request.RequestUri.AbsoluteUri]);
        }
    }
}

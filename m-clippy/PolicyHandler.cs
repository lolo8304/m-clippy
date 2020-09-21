using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace m_clippy
{
    public class PollyHandler : DelegatingHandler
    {
        private readonly IAsyncPolicy<HttpResponseMessage> _policy;

        public PollyHandler(IAsyncPolicy<HttpResponseMessage> policy)
        {
            _policy = policy;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _policy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
        }
    }
}
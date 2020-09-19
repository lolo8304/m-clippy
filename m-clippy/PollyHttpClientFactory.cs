using System.Net.Http;
using Flurl.Http.Configuration;
using Polly;

public class PollyFactory : DefaultHttpClientFactory
{
    private readonly IAsyncPolicy<HttpResponseMessage> _policy;

    public PollyFactory(IAsyncPolicy<HttpResponseMessage> policy)
    {
        _policy = policy;
    }

    public override HttpMessageHandler CreateMessageHandler()
    {
        return new PollyHandler(_policy)
        {
            InnerHandler = base.CreateMessageHandler()
        };
    }
}
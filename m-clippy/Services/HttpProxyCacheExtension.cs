using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace m_clippy.Services
{
    public static class HttpProxyCacheExtension
    {

        public static IFlurlRequest CacheIt(this IFlurlRequest req)
        {
            // do something interesting with req.Settings, req.Headers, req.Url, etc.
            req.Url.
            return req;
        }

        public static IFlurlRequest CacheIt(this Url url) => new FlurlRequest(url).CacheIt();

        public static IFlurlRequest CacheIt(this string url) => new FlurlRequest(url).CacheIt();
    }

    public class ProxyFlurlRequest : IFlurlRequest
    {
        public IFlurlClient Client { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Url Url { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public FlurlHttpSettings Settings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IDictionary<string, object> Headers => throw new NotImplementedException();

        public IDictionary<string, Cookie> Cookies => throw new NotImplementedException();

        public Task<HttpResponseMessage> SendAsync(HttpMethod verb, HttpContent content = null, CancellationToken cancellationToken = default, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            throw new NotImplementedException();
        }
    }




















    rlRequest
    {
        public IFlurlClient Client { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Url Url { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public FlurlHttpSettings Settings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IDictionary<string, object> Headers => throw new NotImplementedException();

        public IDictionary<string, Cookie> Cookies => throw new NotImplementedException();

        public Task<HttpResponseMessage> SendAsync(HttpMethod verb, HttpContent content = null, CancellationToken cancellationToken = default, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            throw new NotImplementedException();
        }
    }

}

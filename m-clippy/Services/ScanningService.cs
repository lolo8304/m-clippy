using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Flurl.Http;
using m_clippy.Models;
using m_clippy.Models.Cart;
using m_clippy.Models.ProductsDetails;
using m_clippy.Models.Purchase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Scotch;

namespace m_clippy.Services
{
    public class ScanningService
    {
        private readonly IConfiguration _configuration;
        private readonly ClippyStorage _clippyStorage;
        
        public static async Task<T> GetJsonAsync<T>(IConfiguration configuration, string url, string fileNameCache)
        {
            var u = configuration["MigrosApiUsername"];
            var p = configuration["MigrosApiPassword"];
            
            // https://github.com/mleech/scotch
            var httpClient = HttpClients.NewHttpClient($"./cassette/{fileNameCache}.json", Startup.GetScotchMode());
            
            var cli = new FlurlClient(httpClient);
            var result = await cli.Request(url).WithHeader("Api-Version", "7")
                .WithHeader("accept-language", "de")
                .WithBasicAuth(u, p)
                .GetJsonAsync<T>();

            return result;
        }

        public ScanningService(ClippyStorage clippyStorage,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _clippyStorage = clippyStorage;
        }

        public async Task<ClippyProductsDetails> GetProductDetails(string userId, string articleId) // Should be EAN code nit article Id!!!
        {
            var user = _clippyStorage.GetUser(userId);
            if (user == null)
            {
                user = new Mocks().GetMockUserById(userId);
                user = _clippyStorage.PutUser(userId, user);
            }
            
            var clippyProductsDetails = new ClippyProductsDetails();
            
            var productDetails =
                await GetJsonAsync<ProductDetails>(_configuration,
                    $"https://hackzurich-api.migros.ch/products.json?ids={articleId}&verbosity=detail", "product");
            
            var productDetailsProducts = productDetails.Products;
            var countriesSet = new HashSet<string>();
            foreach (var productDetail in productDetailsProducts)
            {
                clippyProductsDetails.ProductsAnalyzed++;

                ReportingService.analyseOneProduct(productDetail, user, clippyProductsDetails, countriesSet);
            }
            
            return clippyProductsDetails;
        }
    }
}
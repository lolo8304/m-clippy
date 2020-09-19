using System;
using System.Threading.Tasks;
using Flurl.Http;
using m_clippy.Models.Migros;
using Microsoft.Extensions.Configuration;

namespace m_clippy.Services
{
    public class MigrosService
    {
        private readonly IConfiguration _configuration;

        public MigrosService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {

            return await GetAsync(productId);
        }

        public async Task<Product> GetAsync(string productId)
        {
            var u = _configuration["MigrosApiUsername"];
            var p = _configuration["MigrosApiPassword"];
            return await $"https://hackzurich-api.migros.ch/products/{productId}.json"
                .WithHeader("Api-Version", "7")
                .WithHeader("accept-language", "de")
                .WithBasicAuth(u, p)
                .GetJsonAsync<Product>();
        }



    }
}

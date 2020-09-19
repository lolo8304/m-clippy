using System;
using System.Threading.Tasks;
using Flurl.Http;
using m_clippy.Models;
using m_clippy.Models.Allergens;
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

        public async Task<AllergenList> GetAllergiesAsync()
        {
            var u = _configuration["MigrosApiUsername"];
            var p = _configuration["MigrosApiPassword"];
            var productList = await "https://hackzurich-api.migros.ch/products?feature_facets%5B%5D=MAPI_ALLERGENES&facet_size=0"
                .WithHeader("Api-Version", "7")
                .WithHeader("accept-language", "de")
                .WithBasicAuth(u, p)
                .GetJsonAsync<ProductList>();

            // we map to our own structure
            var allergenList = new AllergenList();
            var terms = productList.Facets.MAPIALLERGENES.Terms;
            foreach (Term6 term in terms) {
                var allergenEntry = new AllergenEntry
                {
                    Code = term.Term,
                    Text = term.Name
                };

                allergenList.list.Add(allergenEntry);
            }

            return allergenList;
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

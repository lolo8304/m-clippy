using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using m_clippy.Models;
using m_clippy.Models.Allergens;
using m_clippy.Models.Migros;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace m_clippy.Services
{
    public class MigrosService
    {
        private readonly IConfiguration _configuration;
        private readonly ClippyStorage _clippyStorage;
        private ILogger<MigrosService> _logger;

        public MigrosService(ClippyStorage clippyStorage, IConfiguration configuration, ILogger<MigrosService> logger)
        {
            _configuration = configuration;
            _clippyStorage = clippyStorage;
            _logger = logger;
        }

        public async Task<AllergenList> GetAllergiesAsync()
        {
            var productList = await ReportingService.GetJsonAsync<ProductList>(_configuration,
                "https://hackzurich-api.migros.ch/products?feature_facets%5B%5D=MAPI_ALLERGENES&facet_size=0");

            // we map to our own structure
            var allergenList = new AllergenList();
            var terms = productList.Facets.MAPIALLERGENES.Terms;
            foreach (var allergenEntry in terms.Select(term => new AllergenEntry
            {
                Code = term.Term,
                Text = term.Name
            }))
            {
                allergenList.list.Add(allergenEntry);
            }

            return allergenList;
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            return await ReportingService.GetJsonAsync<Product>(_configuration,$"https://hackzurich-api.migros.ch/products/{productId}.json");
        }
        
    }
}

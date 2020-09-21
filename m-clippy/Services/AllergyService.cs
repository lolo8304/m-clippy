using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using m_clippy.Models;
using m_clippy.Models.Allergens;
using Microsoft.Extensions.Configuration;

namespace m_clippy.Services
{
    public class AllergyService
    {
        private readonly IConfiguration _configuration;

        public AllergyService(IConfiguration configuration)
        {
            _configuration = configuration;
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
        
    }
}
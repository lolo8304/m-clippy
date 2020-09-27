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

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            return await ReportingService.GetJsonAsync<Product>(_configuration,$"https://hackzurich-api.migros.ch/products/{productId}", "products");
        }
        
    }
}

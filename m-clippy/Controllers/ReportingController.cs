using System.Net;
using System.Threading.Tasks;
using m_clippy.Models;
using m_clippy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

// annotations: FromQuery, FromBody


namespace m_clippy.Controllers
{
    [Route("api/reporting")]
    [ApiController]
    public class ReportingController : ControllerBase
    {
        private readonly ClippyStorage _clippyStorage;
        private readonly ReportingService _reportingService;

        public ReportingController(ClippyStorage clippyStorage, ReportingService reportingService)
        {
            _clippyStorage = clippyStorage;
            _reportingService = reportingService;
        }

        [HttpGet]
        [Route("purchases/{userId}")]
        public async Task<IActionResult> GetPurchase(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new Error($"{StatusCodes.Status400BadRequest}", $"'{nameof(userId)}' cannot be null or whitespace"));
            }

            // Caching because of bad endpoint performances, better would be client side cacing or precalculation in backend
            var clippyProductsDetails = _clippyStorage.GetClippyProductDetails(userId);

            if (clippyProductsDetails == null)
            {
                clippyProductsDetails = await _reportingService.GetPurchases(userId);
                clippyProductsDetails = _clippyStorage.PutClippyProductDetails(userId, clippyProductsDetails);
            }

            var clippyProductsDetailsString = JsonConvert.SerializeObject(clippyProductsDetails);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(clippyProductsDetailsString, "application/json");
        }

        
    }
}

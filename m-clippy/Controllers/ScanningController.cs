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
    [Route("api/scanning")]
    [ApiController]
    public class ScanningController : ControllerBase
    {
        private readonly ClippyStorage _clippyStorage;
        private readonly ScanningService _scanningService;

        public ScanningController(ClippyStorage clippyStorage, ScanningService scanningService)
        {
            _clippyStorage = clippyStorage;
            _scanningService = scanningService;
        }

        [HttpGet]
        [Route("purchases/{userId}")]
        public async Task<IActionResult> GetPurchase(string eanCode, string userId)
        {
            if (string.IsNullOrWhiteSpace(eanCode))
            {
                return BadRequest(new Error($"{StatusCodes.Status400BadRequest}", $"'{nameof(eanCode)}' cannot be null or whitespace"));
            }

            var clippyProductsDetails = await _scanningService.GetProductDetails(userId, eanCode);

            var clippyProductsDetailsString = JsonConvert.SerializeObject(clippyProductsDetails);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(clippyProductsDetailsString, "application/json");
        }

        
    }
}

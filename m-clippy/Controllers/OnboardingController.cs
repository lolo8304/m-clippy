using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using m_clippy.Models;
using m_clippy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

// annotations: FromQuery, FromBody


namespace m_clippy.Controllers
{
    [Route("api/onboarding")]
    [ApiController]
    public class OnboardingController : ControllerBase
    {
        private readonly ClippyStorage _clippyStorage;
        private readonly MigrosService _migrosService;

        public OnboardingController(ClippyStorage clippyStorage, MigrosService migrosService)
        {
            _clippyStorage = clippyStorage;
            _migrosService = migrosService;
        }

        [HttpGet]
        [Route("users/{userId}")]
        public IActionResult GetByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new Error($"{StatusCodes.Status400BadRequest}", $"'{nameof(userId)}' cannot be null or whitespace"));
            }
            var user = _clippyStorage.GetUser(userId);

            if (user == null)
            {
                user = new Mocks().User1();
                user = _clippyStorage.PutUser(userId, user);
            }

            return new JsonResult(user)
            {
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        [HttpGet]
        [Route("products/{productId}")]
        public async Task<IActionResult> GetProductByIdAsync(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                return BadRequest(new Error($"{StatusCodes.Status400BadRequest}", $"'{nameof(productId)}' cannot be null or whitespace"));
            }
            var product = await _migrosService.GetProductByIdAsync(productId);
            var productString = JsonConvert.SerializeObject(product);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(productString, "application/json");
        }

        [HttpGet]
        [Route("purchases/{userId}")]
        public async Task<IActionResult> GetPurchase(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new Error($"{StatusCodes.Status400BadRequest}", $"'{nameof(userId)}' cannot be null or whitespace"));
            }
            var clippyProductsDetails = await _migrosService.getPurchases(userId);
            var clippyProductsDetailsString = JsonConvert.SerializeObject(clippyProductsDetails);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(clippyProductsDetailsString, "application/json");
        }

        [HttpPut]
        [Route("users/{userId}")]
        public IActionResult GetHabitsByUserId(string userId, [FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new Error($"{StatusCodes.Status400BadRequest}", $"'{nameof(userId)}' cannot be null or whitespace"));
            }
            _clippyStorage.PutUser(userId, user);
            return Ok();
        }

        public string ObjectToString(Habits habits)
        {
            return JsonConvert.SerializeObject(habits);
        }
        public Habits StringToObject(string habits)
        {
            return JsonConvert.DeserializeObject<Habits>(habits);
        }

    }
}

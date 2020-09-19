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
        [Route("users/{userId}/habits")]
        public IActionResult GetHabitsByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new Error($"{StatusCodes.Status400BadRequest}", $"'{nameof(userId)}' cannot be null or whitespace"));
            }
            var habit = _clippyStorage.GetHabit(userId);
            if (habit == null) habit = _clippyStorage.PutHabits(userId, new Habits());

            return new JsonResult(habit)
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

        [HttpPut]
        [Route("users/{userId}/habits")]
        public IActionResult GetHabitsByUserId(string userId, [FromBody] Habits habits)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new Error($"{StatusCodes.Status400BadRequest}", $"'{nameof(userId)}' cannot be null or whitespace"));
            }
            _clippyStorage.PutHabits(userId, habits);
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

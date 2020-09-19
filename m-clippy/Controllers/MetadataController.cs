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
    [Route("api/metadata")]
    [ApiController]
    public class MetadataController : ControllerBase
    {
        private readonly ClippyStorage _clippyStorage;
        private readonly MigrosService _migrosService;

        public MetadataController(ClippyStorage clippyStorage, MigrosService migrosService)
        {
            _clippyStorage = clippyStorage;
            _migrosService = migrosService;
        }

        [HttpGet]
        [Route("allergens")]
        public async Task<IActionResult> GetAllergens()
        {
            // caching
            var allergiesString = _clippyStorage.GetAllergies();

            if (allergiesString == null)
            {

                var allergies = await _migrosService.GetAllergiesAsync();
                allergiesString = JsonConvert.SerializeObject(allergies);

                _clippyStorage.PutAllergies(allergiesString);

                Response.StatusCode = (int)HttpStatusCode.OK;
            }

            return Content(allergiesString, "application/json");
        }

    }
}

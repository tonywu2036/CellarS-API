using Microsoft.AspNetCore.Mvc;
using CellarS.Api.Models;
using CellarS.Api.Services;

namespace CellarS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly IRephraseService _rephraseService;

        public AIController(IRephraseService rephraseService)
        {
            _rephraseService = rephraseService;
        }

        [HttpPost("rephrase")]
        public async Task<ActionResult<RephraseResponse>> RephraseText([FromBody] RephraseRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest("Text cannot be empty.");
            }

            var result = await _rephraseService.RephraseTextAsync(request);
            return Ok(result);
        }
    }
}
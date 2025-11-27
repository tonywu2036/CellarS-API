using CellarS.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CellarS.Api.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AiController : ControllerBase
    {
        private readonly BedrockAiService _aiService;
        private readonly ILogger<AiController> _logger;

        public AiController(BedrockAiService aiService, ILogger<AiController> logger)
        {
            _aiService = aiService;
            _logger = logger;
        }

        public class SimplifyRequest
        {
            public string Text { get; set; } = string.Empty;
        }

        public class SimplifyResponse
        {
            public string SimplifiedText { get; set; } = string.Empty;
        }

        [HttpPost("simplify")]
        public async Task<ActionResult<SimplifyResponse>> Simplify([FromBody] SimplifyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest("Text is required.");
            }

            try
            {
                var simplified = await _aiService.SimplifyAsync(request.Text);
                return Ok(new SimplifyResponse { SimplifiedText = simplified });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Bedrock simplify");
                return StatusCode(500, "Error calling AI service.");
            }
        }
    }
}

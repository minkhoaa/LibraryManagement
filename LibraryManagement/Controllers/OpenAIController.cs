using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAIController : ControllerBase
    {
        private readonly OpenAIService _openAIService;

        public OpenAIController(OpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] OpenAIRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Contents))
                return BadRequest("Prompt is required");

            var result = await _openAIService.AskGeminiAsync(request.Contents);
            return Ok(new OpenAIResponse { Reply = result });
        }
    }
}

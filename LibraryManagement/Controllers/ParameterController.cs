using LibraryManagement.Dto.Request;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParameterController : ControllerBase
    {
        private readonly IParameterRepository _parameterRepository;
        public ParameterController(IParameterRepository parameterRepository)
        {
            _parameterRepository = parameterRepository;
        }

        // Endpoint thêm quy định
        [HttpPost("add_parameter")]
        public async Task<IActionResult> addNewParameter([FromBody] ParameterRequest request)
        {
            var result = await _parameterRepository.addParameterAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpont sửa quy định
        [HttpPut("update_parameter/{idParameter}")]
        public async Task<IActionResult> updateParameter([FromBody] ParameterRequest request, Guid idParameter)
        {
            var result = await _parameterRepository.updateParameterAsync(request, idParameter);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Endpoint xóa quy định
        [HttpDelete("delete_parameter/{idParameter}")]
        public async Task<IActionResult> deleteParameter(Guid idParameter)
        {
            var result = await _parameterRepository.deleteParameterAsync(idParameter);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}

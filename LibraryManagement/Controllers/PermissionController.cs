using LibraryManagement.Dto.Request;
using LibraryManagement.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        // Endpoint tạo permisison mới
        [HttpPost("add_permission")]
        public async Task<IActionResult> addNewPermission([FromBody] PermissionRequest request)
        {
            var result = await _permissionService.addPermissionAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpoint xóa permisison
        [HttpDelete("delete_permission/{permissionName}")]
        public async Task<IActionResult> deletePermission(string permissionName)
        {
            var result = await _permissionService.deletePermissionAsync(permissionName);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Endpoint sửa permission
        [HttpPut("update_permission")]
        public async Task<IActionResult> updatePermission([FromBody] PermissionRequest request)
        {
            var result = await _permissionService.updatePermissionAsync(request);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}

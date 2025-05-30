using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Repository.InterFace;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/roles/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // Endpoint tạo role mới
        [HttpPost("add_role")]
        public async Task<IActionResult> addNewRole([FromBody] RoleRequest request)
        {
            var result = await _roleService.addRoleAsync(request);
            if (result.Success)
                return Created("", result);
            return BadRequest(result);
        }

        // Endpoint xóa role
        [HttpDelete("delete_role/{RoleName}")]
        public async Task<IActionResult> deleteRole(string RoleName)
        {
            var result = await _roleService.deleteRoleAsync(RoleName);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        // Endpoint sửa role
        [HttpPut("update_role")]
        public async Task<IActionResult> updateRole([FromBody] RoleRequest request)
        {
            var result = await _roleService.updateRoleAsync(request);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}

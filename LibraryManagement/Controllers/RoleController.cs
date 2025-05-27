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
        public async Task<ApiResponse<RoleResponse>> addNewRole([FromBody] RoleRequest request)
        {
            return await _roleService.addRoleAsync(request);
        }

        // Endpoint xóa role
        [HttpDelete("delete_role")]
        public async Task<ApiResponse<string>> deleteRole(string RoleName)
        {
            return await _roleService.deleteRoleAsync(RoleName);
        }

        // Endpoint sửa role
        [HttpPut("update_role")]
        public async Task<ApiResponse<RoleResponse>> updateRole([FromBody] RoleRequest request)
        {
            return await _roleService.updateRoleAsync(request);
        }
    }
}

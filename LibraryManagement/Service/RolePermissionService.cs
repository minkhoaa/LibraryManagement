using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using LibraryManagement.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Service
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly LibraryManagermentContext _context;

        public RolePermissionService(LibraryManagermentContext context)
        {
            _context = context;
        }

        // Hàm thêm phân quyền
        public async Task<ApiResponse<RolePermissionResponse>> addRolePermissionAsync(RolePermissionRequest request)
        {
            // Kiểm tra role và permission tồn tại
            var role = await _context.Roles.FindAsync(request.RoleName);
            var permission = await _context.Permissions.FindAsync(request.PermissionName);

            if (role == null || permission == null)
            {
                return ApiResponse<RolePermissionResponse>.FailResponse("Role hoặc Permission không tồn tại", 404);
            }

            // Kiểm tra trùng
            bool exists = await _context.RolePermissions.AnyAsync(rp =>
                rp.RoleName == request.RoleName && rp.PermissionName == request.PermissionName);
            if (exists)
            {
                return ApiResponse<RolePermissionResponse>.FailResponse("Phân quyền đã tồn tại", 409);
            }

            var rolePermission = new RolePermission
            {
                RoleName = request.RoleName,
                PermissionName = request.PermissionName
            };

            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync();

            return ApiResponse<RolePermissionResponse>.SuccessResponse("Thêm phân quyền thành công", 201, new RolePermissionResponse
            {
                RoleName = request.RoleName,
                PermissionName = request.PermissionName
            });
        }

        // Hàm xóa phân quyền
        public async Task<ApiResponse<string>> deleteRolePermissionAsync(RolePermissionRequest request)
        {
            var rolePermission = await _context.RolePermissions
                           .FirstOrDefaultAsync(rp => rp.RoleName == request.RoleName && rp.PermissionName == request.PermissionName);

            if (rolePermission == null)
            {
                return ApiResponse<string>.FailResponse("Phân quyền không tồn tại", 404);
            }

            _context.RolePermissions.Remove(rolePermission);
            await _context.SaveChangesAsync();

            return ApiResponse<string>.SuccessResponse("Xóa phân quyền thành công", 200, "");
        }
    }
}

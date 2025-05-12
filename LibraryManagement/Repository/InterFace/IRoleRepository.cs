﻿using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;

namespace LibraryManagement.Repository.InterFace
{
    public interface IRoleRepository
    {
        // Thêm role
        public Task<ApiResponse<RoleResponse>> addRoleAsync(RoleRequest request);

        // Xóa role
        public Task<ApiResponse<string>> deleteRoleAsync(string roleName);

        // Sửa role
        public Task<ApiResponse<RoleResponse>> updateRoleAsync(RoleRequest request);

      
    }
}

﻿using AutoMapper;
using LibraryManagement.Data;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using LibraryManagement.Repository.InterFace;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly LibraryManagermentContext _context;
        private readonly IMapper _mapper;

        public RoleRepository(LibraryManagermentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Hàm thêm role
        public async Task<ApiResponse<RoleResponse>> addRoleAsync(RoleRequest request)
        {
            var newRole = await _context.Roles.FirstOrDefaultAsync(role => role.RoleName == request.RoleName);
            if (newRole == null)
            {
                newRole = new Role
                {
                    RoleName = request.RoleName,
                    Description = request.Description
                };
                _context.Roles.Add(newRole);
                await _context.SaveChangesAsync();
                var roleResponse = _mapper.Map<RoleResponse>(newRole);
                return ApiResponse<RoleResponse>.SuccessResponse("Thêm role thành công", 201, roleResponse);
            }
            return ApiResponse<RoleResponse>.FailResponse("Role đã tồn tại", 409);
        }

        // Hàm xóa role
        public async Task<ApiResponse<string>> deleteRoleAsync(string roleName)
        {
            var deleteRole = await _context.Roles.FirstOrDefaultAsync(role => role.RoleName == roleName);
            if (deleteRole == null)
            {
                return ApiResponse<string>.FailResponse("Không tìm thấy role", 404);
            }
            _context.Roles.Remove(deleteRole);
            await _context.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("Xóa role thành công", 200, roleName);
        }

        // Hàm sửa nội dung role
        public async Task<ApiResponse<RoleResponse>> updateRoleAsync(RoleRequest request)
        {
            var updateRole = await _context.Roles.FirstOrDefaultAsync(role => role.RoleName == request.RoleName);
            if (updateRole == null)
            {
                return ApiResponse<RoleResponse>.FailResponse("Không tìm thấy role", 404);
            }

            updateRole.Description = request.Description;

            _context.Roles.Update(updateRole);
            await _context.SaveChangesAsync();
            var roleResponse = _mapper.Map<RoleResponse>(updateRole);
            return ApiResponse<RoleResponse>.SuccessResponse("Thay đổi role thành công", 200, roleResponse);
        }
    }
}

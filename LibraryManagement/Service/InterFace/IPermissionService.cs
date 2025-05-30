using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;

namespace LibraryManagement.Service.Interface
{
    public interface IPermissionService
    {
        public Task<ApiResponse<PermissionResponse>> addPermissionAsync(PermissionRequest request);
        public Task<ApiResponse<string>> deletePermissionAsync(string permissionName);
        public Task<ApiResponse<PermissionResponse>> updatePermissionAsync(PermissionRequest request);
    }
}

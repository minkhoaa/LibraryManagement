using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Helpers;
using LibraryManagement.Service.Interface;

namespace LibraryManagement.Service
{
    public class OverdueReportService : IOverdueReportService
    {
        public Task<ApiResponse<OverdueReportResponse>> addOverdueReportAsync(OverdueReportRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> deleteOverReportAsync(Guid idOverReport)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<OverdueReportResponse>> updateOverdueReportAsync(OverdueReportRequest request, Guid idOverReport)
        {
            throw new NotImplementedException();
        }
    }
}

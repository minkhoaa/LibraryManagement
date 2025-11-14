namespace LibraryManagement.Repository.InterFace
{
    public interface IUpLoadImageFileService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}

namespace LibraryManagement.Repository.InterFace
{
    public interface IUpLoadImageFileRepository
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}

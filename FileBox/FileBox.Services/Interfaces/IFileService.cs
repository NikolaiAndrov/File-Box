namespace FileBox.Services.Interfaces
{
    using Microsoft.AspNetCore.Http;

    public interface IFileService
    {
        Task UploadFilesAsync(ICollection<IFormFile> files);
    }
}

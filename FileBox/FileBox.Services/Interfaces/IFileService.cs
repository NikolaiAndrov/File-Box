namespace FileBox.Services.Interfaces
{
    using Microsoft.AspNetCore.Http;

    public interface IFileService
    {
        Task<ICollection<string>> UploadFilesAsync(ICollection<IFormFile> files);
    }
}

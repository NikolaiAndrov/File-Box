namespace FileBox.Services.Interfaces
{
    using Microsoft.AspNetCore.Http;

    public interface IFileService
    {
        Task UploadFiles(ICollection<IFormFile> files);
    }
}

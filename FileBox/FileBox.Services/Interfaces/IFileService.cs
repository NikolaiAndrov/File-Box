namespace FileBox.Services.Interfaces
{
    using FileBox.ViewModels.Files;
    using Microsoft.AspNetCore.Http;

    public interface IFileService
    {
        Task<ICollection<string>> UploadFilesAsync(ICollection<IFormFile> files);

        Task<ICollection<string>> AreAnyExistingFilesAsync(ICollection<IFormFile> files);

        Task<ICollection<FileViewModel>> GetAllFilesForViewingAsync();

        Task<bool> IsFileExistingById(int id);

        Task DeleteAsync(int id);
    }
}

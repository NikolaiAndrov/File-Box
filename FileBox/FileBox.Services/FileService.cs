namespace FileBox.Services
{
    using FileBox.Data;
    using FileBox.Services.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class FileService : IFileService
    {
        private readonly FileBoxDbContext dbContext;

        public FileService(FileBoxDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task UploadFiles(ICollection<IFormFile> files)
        {
            throw new NotImplementedException();
        }
    }
}

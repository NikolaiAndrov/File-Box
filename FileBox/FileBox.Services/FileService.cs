namespace FileBox.Services
{
    using FileBox.Data;
    using FileBox.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Data.Models;
    using System.IO;

    public class FileService : IFileService
    {
        private readonly FileBoxDbContext dbContext;

        public FileService(FileBoxDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<string>> UploadFilesAsync(ICollection<IFormFile> files)
        {
            ICollection<string> filesUploaded = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    string entireFileName = Path.GetFileName(file.FileName);
                    int dotIndex = entireFileName.LastIndexOf('.');
                    string name = entireFileName.Substring(0, dotIndex);
                    string extension = entireFileName.Substring(dotIndex + 1);

                    Data.Models.File newFile = new Data.Models.File
                    {
                        Name = name,
                        Extension = extension,
                        Size = file.Length,
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        newFile.Data = memoryStream.ToArray();
                    }

                    await this.dbContext.Files.AddAsync(newFile);
                    await this.dbContext.SaveChangesAsync();

                    string fileNameUploaded = $"{name}.{extension}";
                    filesUploaded.Add(fileNameUploaded);
                }
            }

            return filesUploaded;
        }
    }
}

namespace FileBox.Services
{
    using FileBox.Data;
    using FileBox.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Data.Models;
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Data.SqlClient;
    using System.Data;
    using static Common.ApplicationMessages;

    public class FileService : IFileService
    {
        private readonly FileBoxDbContext dbContext;
        private readonly string connectionString;

        public FileService(FileBoxDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ICollection<string>> AreAnyExistingFiles(ICollection<IFormFile> files)
        {
            ICollection<string> existingFiles = new List<string>();

            foreach (var file in files)
            {
                string entireFileName = Path.GetFileName(file.FileName);
                int dotIndex = entireFileName.LastIndexOf('.');
                string name = entireFileName.Substring(0, dotIndex);
                string extension = entireFileName.Substring(dotIndex + 1);

                if (await this.dbContext.Files.AsNoTracking().AnyAsync(f => f.Name == name && f.Extension == extension))
                {
                    existingFiles.Add($"{name}.{extension}");
                }
            }

            return existingFiles;
        }

        public async Task<ICollection<string>> UploadFilesAsync(ICollection<IFormFile> files)
        {
            ICollection<string> filesUploaded = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        foreach (var file in files)
                        {
                            if (file.Length > 0)
                            {
                                string entireFileName = Path.GetFileName(file.FileName);
                                int dotIndex = entireFileName.LastIndexOf('.');
                                string name = entireFileName.Substring(0, dotIndex);
                                string extension = entireFileName.Substring(dotIndex + 1);

                                using (var command = new SqlCommand("INSERT INTO Files (Name, Extension, Size, Data) VALUES (@Name, @Extension, @Size, @Data)", connection, (SqlTransaction)transaction))
                                {
                                    command.Parameters.AddWithValue("@Name", name);
                                    command.Parameters.AddWithValue("@Extension", extension);
                                    command.Parameters.AddWithValue("@Size", file.Length);
                                    command.Parameters.Add("@Data", SqlDbType.VarBinary, -1).Value = file.OpenReadStream();

                                    await command.ExecuteNonQueryAsync();
                                }

                                string fileNameUploaded = $"{name}.{extension}";
                                filesUploaded.Add(fileNameUploaded);
                            }
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new InvalidOperationException(UnexpectedErrorMessage, ex);
                    }
                }
            }

            return filesUploaded;
        }

    }
}

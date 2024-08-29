namespace FileBox.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FileBoxDbContext : DbContext
    {
        public FileBoxDbContext(DbContextOptions<FileBoxDbContext> options)
            : base(options)
        {

        }

        public DbSet<File> Files { get; set; } = null!;
    }

}

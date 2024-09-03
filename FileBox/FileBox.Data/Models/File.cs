namespace FileBox.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class File
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Extension { get; set; } = null!;

        [Required]
        public string ContentType { get; set; } = null!;

        [Required]
        public long Size { get; set; }

        [Required]
        public byte[] Data { get; set; } = null!;
    }
}

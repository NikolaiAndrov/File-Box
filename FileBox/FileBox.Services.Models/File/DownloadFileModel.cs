namespace FileBox.Services.Models.File
{
    public class DownloadFileModel
    {
        public string FileName { get; set; } = null!;

        public string ContentType { get; set; } = null!;

        public byte[] Data { get; set; } = null!;
    }
}

namespace FileBox.Controllers
{
    using FileBox.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using static Common.ApplicationMessages;
    using static Common.GlobalConstants;

    public class FileController : Controller
    {
        private readonly IFileService fileService;

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                ModelState.AddModelError("", "Please select at least one file to upload.");
                return View();
            }

            ICollection<string> filesUploaded = new List<string>();

            try
            {
                filesUploaded = await this.fileService.UploadFilesAsync(files);
            }
            catch (Exception)
            {
                this.TempData[Error] = ErrorMessageFilesUploaded + string.Join(", ", filesUploaded);
                return this.RedirectToAction("Index", "Home");
            }

            this.TempData[Success] = SuccessMessageFilesUploaded + string.Join(", ", filesUploaded);
            return this.RedirectToAction("Index", "Home");
        }
    }
}

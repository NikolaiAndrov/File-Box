namespace FileBox.Controllers
{
    using FileBox.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

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

            try
            {
                await this.fileService.UploadFilesAsync(files);
            }
            catch (Exception)
            {
               // this.TempData
            }


            return this.RedirectToAction("Index", "Home");
        }
    }
}

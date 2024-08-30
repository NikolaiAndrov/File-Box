namespace FileBox.Controllers
{
    using FileBox.Services.Interfaces;
    using FileBox.ViewModels.Files;
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
                this.TempData[Error] = SelectFilesMessage;
                return View();
            }

            ICollection<string> existingFiles = await this.fileService.AreAnyExistingFilesAsync(files);

            if (existingFiles.Any())
            {
                this.TempData[Error] = ExistingFilesMessage + string.Join(", ", existingFiles);
                return this.View();
            }

            ICollection<string> filesUploaded = new List<string>();

            try
            {
                filesUploaded = await this.fileService.UploadFilesAsync(files);
            }
            catch (Exception ex)
            {
                this.TempData[Error] = ex.Message;
                return this.RedirectToAction("Index", "Home");
            }

            this.TempData[Success] = SuccessMessageFilesUploaded + string.Join(", ", filesUploaded);
            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            ICollection<FileViewModel> files;

            try
            {
                files = await this.fileService.GetAllFilesForViewingAsync();
            }
            catch (Exception)
            {
                this.TempData[Error] = UnexpectedErrorMessage;
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(files);
        }
    }
}

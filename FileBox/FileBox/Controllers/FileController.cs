namespace FileBox.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class FileController : Controller
    {
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

            return this.RedirectToAction("Index", "Home");
        }
    }
}

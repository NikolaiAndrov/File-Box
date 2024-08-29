namespace FileBox.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class FileController : Controller
    {
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
    }
}

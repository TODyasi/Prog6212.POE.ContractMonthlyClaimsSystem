using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Prog6212.POE.ContractMonthlyClaimsSystem.Controllers.Documents
{
    public class UploadDocumentsController : Controller
    {
        // Declaring a read-only variable to hold the hosting environment
        public readonly IWebHostEnvironment _env;

        // Constructor to initialize the controller with the hosting environment
        public UploadDocumentsController(IWebHostEnvironment env)
        {
            _env = env; // Assigning the passed environment to the _env variable
        }

        // GET method to display the upload form
        public IActionResult Index()
        {
            return View(); // Returns the view for the index action
        }

        // POST method to handle file uploads
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file) // Accepting a file from the form
        {
            const long maxFileSize = 2 * 1024 * 1024;

            // Check if no file was uploaded
            if (file == null)
            {
                ViewBag.Message = "No file was uploaded.";
                return View(); // Return the view without processing
            }

            // Check if the uploaded file is empty
            if (file.Length == 0)
            {
                ViewBag.Message = "Uploaded file is empty.";
                return View(); // Return the view without processing
            }

            // Check if the file size exceeds the maximum allowed size
            if (file.Length > maxFileSize)
            {
                ViewBag.Message = "File size exceeds the maximum limit of 2 MB.";
                return View(); // Return the view without processing
            }

            //path to the uploads folder using the web root path
            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

            // Check if the uploads folder exists, and create it if it doesn't
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder); // Create the uploads directory
            }

            // Get the file name without extension and the file's extension
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName);

            // Create a unique file name by appending a timestamp
            string uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmm}{fileExtension}";

            // Combine the uploads folder path with the unique file name to get the full save path
            string fileSavePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Create a file stream to save the uploaded file
            using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
            {
                // Copy the uploaded file's content to the created file stream
                await file.CopyToAsync(stream);
            }

            // Set a success message with the name of the uploaded file
            ViewBag.Message = $"{uniqueFileName} uploaded successfully.";

            return View(); // Return the view after the upload
        }
    }
}

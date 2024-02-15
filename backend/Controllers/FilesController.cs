using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace photo_add.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly FileService _fileService;

        public FilesController(FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> ListAllBlobs(string fileName, string emailAddress)
        {
            var result = await _fileService.ListAsync(fileName, emailAddress);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, string emailAddress)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (string.IsNullOrWhiteSpace(emailAddress))
                return BadRequest("Email address is required.");

            var result = await _fileService.UploadAsync(file, emailAddress);

            if (!result.Error)
            {
                await MyFunctions.Run(file.FileName, emailAddress);

                return Ok(result);
            }
            else
            {
                // Error occurred during upload
                return StatusCode(500, "An error occurred while uploading the file.");
            }
        }


    }
}

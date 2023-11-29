using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;


namespace PassportVerification.Controllers
{
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        [HttpPost]
        [Route("UploadFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(string))]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellation)
        {
            var result = await WriteFile(file);
            return Ok(result);
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            string filename = "";
            try
            {

                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
                using(var stream=new FileStream(exactpath,FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

            }
            catch (Exception ex)
            {

            }
            return filename;
        }

        [HttpGet]
        [Route("DownloadFile/{filename}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(string))]
        public IActionResult DownloadFile(string filename)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);

                if (System.IO.File.Exists(filePath))
                {
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, "application/octet-stream", filename);
                }
                else
                {
                    return NotFound("File not found");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}

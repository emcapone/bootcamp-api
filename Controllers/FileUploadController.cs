using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using bootcamp_api.Models;
using bootcamp_api.Services;
using System.Net.Http.Headers;

namespace bootcamp_api.Controllers;

[ApiController]
[SwaggerResponse(500, "An error occurred while processing the request", typeof(String))]
[Route("[controller]")]
public class FileUploadController : ControllerBase
{
    /// <summary>
    /// Uploads one file
    /// </summary>
    /// <remarks>
    /// The body accepts FormData containing at least one file.
    /// Only the first file will be uploaded.
    /// The file is saved to Resources/Images/{user_id}/{folder}
    /// </remarks>
    /// <param name="user_id">The current user's ID</param>
    /// <param name="folder">The folder the new file should be contained in.</param>
    [HttpPost("{user_id}/{folder}"), DisableRequestSizeLimit]
    [SwaggerResponse(200, "A new file was uploaded.", typeof(String))]
    [SwaggerResponse(400, "The request is invalid.", typeof(ProblemDetails))]
    public IActionResult Upload(int user_id, string folder)
    {
        try
        {
            var file = Request.Form.Files[0];
            var folderName = Path.Combine("Resources", "Files", user_id.ToString(), folder);
            System.IO.Directory.CreateDirectory(folderName);
            ClearDirectory(folderName);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok(new { dbPath });
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
    }

    public static void ClearDirectory(string path)
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(path);
        foreach (FileInfo file in di.EnumerateFiles())
        {
            file.Delete(); 
        }
        foreach (DirectoryInfo dir in di.EnumerateDirectories())
        {
            dir.Delete(true); 
        }
    }
}
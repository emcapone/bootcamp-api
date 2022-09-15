using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Dto;

namespace bootcamp_api.Controllers
{

    /// <summary>
    /// Handles incoming HTTP requests for uploading files
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FileUploadController : ControllerBase
    {
        /// <summary>
        /// Uploads one file
        /// </summary>
        /// <remarks>
        /// The body accepts FormData containing at least one file.
        /// Only the first file will be uploaded.
        /// The file is saved to Resources/Users/{user_id}/{pet_id}/{folder}
        /// </remarks>
        /// <param name="user_id">The current user's ID</param>
        /// <param name="pet_id">The current pet's ID</param>
        /// <param name="folder">The folder the new file should be contained in.</param>
        [HttpPost("{user_id}/{pet_id}/{folder}"), DisableRequestSizeLimit]
        [ProducesResponseType(typeof(FileLink), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult Upload(int user_id, int pet_id, string folder)
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Users", user_id.ToString(), pet_id.ToString(), folder);
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
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Clears a given directory
        /// </summary>
        /// <param name="path">The path of the directory to clear</param>
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
}
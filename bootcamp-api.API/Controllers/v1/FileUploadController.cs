using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Dto;
using bootcamp_api.Services;
using Azure.Storage.Blobs.Models;
using bootcamp_api.Exceptions;

namespace bootcamp_api.Controllers
{

    /// <summary>
    /// Handles incoming HTTP requests for uploading files
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Consumes("multipart/form-data")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FileUploadController : ControllerBase
    {

        private readonly IFileUploadService _fileService;

        /// <summary>
        /// FileUploadController constructor
        /// </summary>
        public FileUploadController(IFileUploadService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Uploads one non-empty file
        /// </summary>
        /// <remarks>
        /// The body accepts FormData containing at least one file.
        /// Only the first file will be uploaded.
        /// The file is saved to the path pet-files/{user_id}/{pet_id}/{folder}
        /// </remarks>
        /// <param name="user_id">The current user's ID</param>
        /// <param name="pet_id">The current pet's ID</param>
        /// <param name="folder">The folder the new file should be contained in. This value will be turned into a slug.</param>
        [HttpPost("{user_id}/{pet_id}/{folder}"), DisableRequestSizeLimit]
        [ProducesResponseType(typeof(FileLink), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Upload(int user_id, int pet_id, string folder)
        {
            try
            {
                return Ok(await _fileService.UploadAsync(Request.Form.Files[0], user_id, pet_id, folder));
            }
            catch (EmptyFileException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

    }
}
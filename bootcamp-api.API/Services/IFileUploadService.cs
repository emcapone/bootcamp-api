using Azure.Storage.Blobs.Models;
using Dto;

namespace bootcamp_api.Services
{
    public interface IFileUploadService
    {
        public Task<FileLink> UploadAsync(IFormFile file, string user_id, int pet_id, string folder);
    }
}

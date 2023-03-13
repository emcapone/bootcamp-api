using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System.Reflection.Metadata;
using Azure;
using Dto;
using bootcamp_api.Exceptions;

namespace bootcamp_api.Services
{
    public class FileUploadService: IFileUploadService
    {
        private readonly BlobContainerClient _blobContainerClient;
        public FileUploadService(BlobServiceClient blobServiceClient)
        {
            _blobContainerClient = blobServiceClient.GetBlobContainerClient("pet-files");
        }

        public async Task<FileLink> UploadAsync(IFormFile file, string user_id, int pet_id, string folder)
        {

            if (file.Length <= 0)
            {
                throw new EmptyFileException();
            }

            var path = $"{user_id}/{pet_id}/{Slugify(folder)}/";
            var fileName = path + string.Join("_", file.FileName.Split(System.IO.Path.GetInvalidFileNameChars()));

            await EmptyFolder(path);

            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            var header = new BlobHttpHeaders();
            header.ContentType = file.ContentType;
            await blobClient.UploadAsync(file.OpenReadStream(), header);

            return new FileLink {
                DbPath = blobClient.Uri.ToString()
            };
        }

        private async Task EmptyFolder(string folderPath)
        {
            var blobItems = _blobContainerClient.GetBlobsAsync(prefix: folderPath);
            await foreach (BlobItem blobItem in blobItems)
            {
                BlobClient blobClient = _blobContainerClient.GetBlobClient(blobItem.Name);
                await blobClient.DeleteIfExistsAsync();
            }
        }

        public static string Slugify(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            string str = System.Text.Encoding.ASCII.GetString(bytes).ToLower();
            str = System.Text.RegularExpressions.Regex.Replace(str, @"[^a-z0-9\s-]", ""); // Remove all non valid chars          
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s", "-"); // //Replace spaces by dashes
            return str;
        }
    }
}

using photo_add.Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Azure.Storage;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace photo_add
{
    public class FileService
    {
        private readonly string _storageAccount = "datafortress";
        private readonly string _accessKey = "7iY7R7fznxV6CeJqVS4rMblNVo7gLYV+VSrV5M285rdVhAnAF4QW/prE87M4keFOE1FUz/WYAym6+AStjO7HtA==";
        private readonly BlobContainerClient _filesContainer;

        public FileService()
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _accessKey);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesContainer = blobServiceClient.GetBlobContainerClient("files");
        }

        public async Task<List<FileModel>> ListAsync(string fileName, string emailAddress)
        {
            List<FileModel> files = new List<FileModel>();

            string containerUri = _filesContainer.Uri.ToString(); // Getting the container URI

            await foreach (var file in _filesContainer.GetBlobsAsync())
            {
                // Getting the full URI of each file
                var fullUri = $"{containerUri}/{file.Name}?fileName={fileName}&emailAddress={emailAddress}";

                files.Add(new FileModel
                {
                    Uri = fullUri,
                    Name = file.Name,
                    ContentType = file.Properties.ContentType
                });
            }

            return files;
        }


        public async Task<BlobResponseDto> UploadAsync(IFormFile blob, string emailAddress)
        {
            BlobResponseDto blobResponseDto = new();
            BlobClient client = _filesContainer.GetBlobClient(blob.FileName);

            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data);
            }

            blobResponseDto.Status = $"File {blob.FileName} Uploaded Successfully!!!";
            blobResponseDto.Error = false;
            blobResponseDto.Blob.Uri = client.Uri.AbsoluteUri;
            blobResponseDto.Blob.Name = blob.Name;

            // Assuming you want to send the email here

            return blobResponseDto;
        }
    }
}

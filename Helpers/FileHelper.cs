using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using MusicApi.Models;

namespace MusicApi.Helpers
{
    public enum EContainerNames
    {
        None = 0,
        SongCover,
        AudioFile
    }

    public static class FileHelper
    {
        private static Dictionary<EContainerNames, string> containers;

        static FileHelper()
        {
            containers = new Dictionary<EContainerNames, string> {
                { EContainerNames.None, ""},
                { EContainerNames.SongCover, "songscover" },
                { EContainerNames.AudioFile, "audiofiles" }
            };
        }

        public static async Task<string> UploadImage(IFormFile file)
        {
            //string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicstorageaccount2021;AccountKey=sQsQOaTxbWy1n0vcj6MGddjMDOwwrkAPFURUc7F9egG3QbJN0Mjz9dWoE1hu/ZK++ExbIKtE24wtx8Xy0ygq+w==;EndpointSuffix=core.windows.net";
            //string containerName = "songscover";

            //BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            //BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            //var memoryStream = new MemoryStream();
            //await file.CopyToAsync(memoryStream);
            //memoryStream.Position = 0;
            //await blobClient.UploadAsync(memoryStream);
            //return blobClient.Uri.AbsoluteUri;
            return await UploadFile(file, EContainerNames.SongCover);
        }

        public static async Task<string> UploadAudioFile(IFormFile file)
        {
            //string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicstorageaccount2021;AccountKey=sQsQOaTxbWy1n0vcj6MGddjMDOwwrkAPFURUc7F9egG3QbJN0Mjz9dWoE1hu/ZK++ExbIKtE24wtx8Xy0ygq+w==;EndpointSuffix=core.windows.net";
            //string containerName = "audiofiles";

            //BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            //BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            //var memoryStream = new MemoryStream();
            //await file.CopyToAsync(memoryStream);
            //memoryStream.Position = 0;
            //await blobClient.UploadAsync(memoryStream);
            //return blobClient.Uri.AbsoluteUri;
            return await UploadFile(file, EContainerNames.AudioFile);
        }

        public static async Task<string> UploadFile(IFormFile file, EContainerNames containerName)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicstorageaccount2021;AccountKey=sQsQOaTxbWy1n0vcj6MGddjMDOwwrkAPFURUc7F9egG3QbJN0Mjz9dWoE1hu/ZK++ExbIKtE24wtx8Xy0ygq+w==;EndpointSuffix=core.windows.net";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containers[containerName]);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            return blobClient.Uri.AbsoluteUri;

        }
    }
}

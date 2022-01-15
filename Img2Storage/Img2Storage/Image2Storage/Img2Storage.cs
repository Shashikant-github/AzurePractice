using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Azure.Storage.Blobs;
using Image2Storage.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Image2Storage
{
    public static class Img2Stoarage
    {
        [FunctionName("Img2Storage")]
        public static async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Blob("ShashiImages",FileAccess.ReadWrite,Connection=Literals.AzureWebJobsStorage)] CloudBlobContainer blobContainer,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic dataRequest = JsonConvert.DeserializeObject<Image>(requestBody);
            //name = name ?? data?.name;

            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

            //return new OkObjectResult(responseMessage);

            var newID = new Guid();
            var blogName = $"{newID}.jpg";
            var cloudBlockBlog = blobContainer.GetBlockBlobReference(blogName);
            var cloudBlobContainer = blobContainer.CreateIfNotExistsAsync();

            var photoInBytes = Convert.FromBase64String(dataRequest.Photo);
            await cloudBlockBlog.UploadFromByteArrayAsync(photoInBytes, 0, photoInBytes);

            log.LogInformation($"Upload JPEG{newID}.jpg added");

            return $"{cloudBlobContainer}Successfully added{newID}.jpg";
        }
    }
}

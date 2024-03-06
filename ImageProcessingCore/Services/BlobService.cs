using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageProcessingCore.Configuration;
using Microsoft.Extensions.Options;

namespace ImageProcessingCore.Services;

public class BlobService : IBlobService
{
    private readonly BlobContainerClient _containerClient;

    public BlobService(IOptions<BlobStorageOptions> blobStorageOptions)
    {
        var options = blobStorageOptions.Value;
        var serviceClient = new BlobServiceClient(options.ConnectionString);
        _containerClient = serviceClient.GetBlobContainerClient(options.ContainerName);
    }

    public async Task<string> UploadAsync(string fileName, Stream stream)
    {
        await _containerClient.UploadBlobAsync(fileName, stream);
        return $"{_containerClient.Uri}/{fileName}";
    }

    public async Task<BlobDownloadInfo> GetAsync(string fileName) =>
        (await _containerClient.GetBlobClient(fileName).DownloadAsync()).Value;
}
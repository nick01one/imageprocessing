using Azure.Storage.Blobs.Models;

namespace ImageProcessingCore.Services;

public interface IBlobService
{
    Task<string> UploadAsync(string fileName, Stream stream);
    Task<BlobDownloadInfo> GetAsync(string fileName);
}
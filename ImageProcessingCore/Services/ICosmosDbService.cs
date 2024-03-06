using ImageProcessingCore.Models;

namespace ImageProcessingCore.Services;

public interface ICosmosDbService
{
    Task SaveFileTaskAsync(FileTask fileTask);
    Task<FileTask> GetFileTaskAsync(Guid id);
    Task UpdateFileTaskAsync(FileTask fileTask);
}
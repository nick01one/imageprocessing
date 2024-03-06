using ImageProcessingCore.Configuration;
using ImageProcessingCore.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Container = Microsoft.Azure.Cosmos.Container;

namespace ImageProcessingCore.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(IOptions<CosmosDbOptions> cosmosDbOptions)
    {
        var options = cosmosDbOptions.Value;
        var cosmosClient = new CosmosClient(options.Account, options.Key);
        _container = cosmosClient.GetContainer(options.DatabaseName, options.ContainerName);
    }
    
    public async Task SaveFileTaskAsync(FileTask fileTask) => await _container.CreateItemAsync(fileTask);
    
    public async Task<FileTask> GetFileTaskAsync(Guid id)
    {
        var partitionKeyValue = id.ToString();
        var response = await _container.ReadItemAsync<FileTask>(partitionKeyValue, new PartitionKey(partitionKeyValue));
        return response.Resource;
    }
    
    public async Task UpdateFileTaskAsync(FileTask fileTask) =>
        await _container.UpsertItemAsync(fileTask, new PartitionKey(fileTask.Id.ToString()));
}
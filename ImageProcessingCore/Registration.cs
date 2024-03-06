using ImageProcessingCore.Configuration;
using ImageProcessingCore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageProcessingCore;

public static class Registration
{
    public static void RegisterCoreServices(this IServiceCollection collection)
    {
        collection.AddSingleton<IBlobService, BlobService>();
        collection.AddSingleton<ICosmosDbService, CosmosDbService>();
        collection.AddSingleton<ITopicService, TopicService>();
    }

    public static void RegisterOptions(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddOptions<BlobStorageOptions>().Bind(configuration.GetSection(BlobStorageOptions.SectionKey));
        collection.AddOptions<CosmosDbOptions>().Bind(configuration.GetSection(CosmosDbOptions.SectionKey));
        collection.AddOptions<TopicOptions>().Bind(configuration.GetSection(TopicOptions.SectionKey));
    }
}
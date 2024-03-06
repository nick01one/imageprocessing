using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs.Models;
using ImageProcessingCore.Configuration;
using ImageProcessingCore.Models;
using ImageProcessingCore.Services;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace ImageProcessingService;

public class ImageProcessor : BackgroundService
{
    private readonly ServiceBusProcessor _processor;
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IBlobService _blobService;
    private readonly ILogger<ImageProcessor> _logger;
    private readonly int _maxFileSize;

    public ImageProcessor(
        IOptions<TopicOptions> topicOptions,
        ICosmosDbService cosmosDbService,
        IBlobService blobService,
        IConfiguration configuration,
        ILogger<ImageProcessor> logger)
    {
        _cosmosDbService = cosmosDbService;
        _blobService = blobService;
        _logger = logger;
        _maxFileSize = configuration.GetValue<int>("MaxFileSize");
        
        var options = topicOptions.Value;
        var client = new ServiceBusClient(options.ConnectionString);
        _processor = client.CreateProcessor(
            options.TopicName,
            options.SubscriptionName,
            new ServiceBusProcessorOptions());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;
        await _processor.StartProcessingAsync(stoppingToken);
    }
    
    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        var taskId = args.Message.Body.ToString();
        var fileTask = await _cosmosDbService.GetFileTaskAsync(new Guid(taskId));
        _logger.LogInformation("Received: {FileTaskId}", fileTask.Id);

        var file = await _blobService.GetAsync(fileTask.FileName);
        if (file.ContentLength > _maxFileSize)
        {
            await UpdateFileTaskAsync(fileTask, TaskState.Error);
        }
        else
        {
            await UpdateFileTaskAsync(fileTask, TaskState.InProgress);

            var image = CreateRotatedImage(file);
            fileTask.ProcessedFilePath = await _blobService.UploadAsync($"rotated_{fileTask.FileName}", ImageToStream(image));
            
            await UpdateFileTaskAsync(fileTask, TaskState.Done);
        }
        
        _logger.LogInformation("Process completed: {FileTaskId} with status {FileTaskTaskState}", fileTask.Id, fileTask.TaskState);
        await args.CompleteMessageAsync(args.Message);
    }

    private static Image CreateRotatedImage(BlobDownloadInfo file)
    {
        var image = Image.Load(file.Content);
        image.Mutate(c => c.Rotate(RotateMode.Rotate180));
        return image;
    }

    private static MemoryStream ImageToStream(Image image) {
        var memoryStream = new MemoryStream();
        image.Save(memoryStream, new JpegEncoder());
        memoryStream.Position = 0;
        return memoryStream;
    }

    private async Task UpdateFileTaskAsync(FileTask fileTask, TaskState taskState)
    {
        fileTask.TaskState = taskState;
        await _cosmosDbService.UpdateFileTaskAsync(fileTask);
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception.Message);
        return Task.CompletedTask;
    }
}
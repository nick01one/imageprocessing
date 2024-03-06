using ImageProcessingCore.Models;
using ImageProcessingCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureImageFilesProcessor.Controllers;

[ApiController]
[Route("api/image")]
public class ImageController : ControllerBase
{
    private readonly IBlobService _blobService;
    private readonly ICosmosDbService _cosmosDbService;
    private readonly ITopicService _topicService;

    public ImageController(
        IBlobService blobService,
        ICosmosDbService cosmosDbService,
        ITopicService topicService)
    {
        _blobService = blobService;
        _cosmosDbService = cosmosDbService;
        _topicService = topicService;
    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> UploadAsync(IFormFile file)
    {
        var filePath = await _blobService.UploadAsync(file.FileName, await FileToStreamAsync(file));
        var fileTask = CreateFileTask(file.FileName, filePath);
        await _cosmosDbService.SaveFileTaskAsync(fileTask);
        await _topicService.SendMessageAsync(fileTask.Id);        
        return Ok(fileTask.Id);
    }
    
    private static async Task<MemoryStream> FileToStreamAsync(IFormFile file)
    {
        var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }

    private static FileTask CreateFileTask(string fileName, string filePath) =>
        new()
        {
            FileName = fileName,
            OriginalFilePath = filePath,
            Id = Guid.NewGuid(),
            TaskState = TaskState.Created
        };
}
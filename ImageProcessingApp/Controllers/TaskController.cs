using ImageProcessingCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureImageFilesProcessor.Controllers;

[ApiController]
[Route("api/task")]
public class TaskController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;

    public TaskController(ICosmosDbService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }

    [HttpGet]
    [Route("{taskId}")]
    public async Task<IActionResult> GetAsync(Guid taskId)
    {
        var fileTask = await _cosmosDbService.GetFileTaskAsync(taskId);
        return Ok(fileTask.ProcessedFilePath ?? fileTask.TaskState.ToString());
    }
}
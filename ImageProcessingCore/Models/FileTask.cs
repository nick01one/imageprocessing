using Newtonsoft.Json;

namespace ImageProcessingCore.Models;

public class FileTask
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }

    [JsonProperty(PropertyName = "fileName")]
    public required string FileName { get; set; }

    [JsonProperty(PropertyName = "originalFilePath")]
    public required string OriginalFilePath { get; set; }

    [JsonProperty(PropertyName = "processedFilePath")]
    public string? ProcessedFilePath { get; set; }

    [JsonProperty(PropertyName = "taskState")]
    public TaskState TaskState { get; set; }
}
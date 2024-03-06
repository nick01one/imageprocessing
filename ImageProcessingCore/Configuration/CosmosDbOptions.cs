namespace ImageProcessingCore.Configuration;

public class CosmosDbOptions
{
    public const string SectionKey = "CosmosDb";
    public string Account { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}
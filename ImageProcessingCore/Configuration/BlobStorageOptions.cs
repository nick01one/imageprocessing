namespace ImageProcessingCore.Configuration;

public class BlobStorageOptions
{
    public const string SectionKey = "BlobStorage";
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}
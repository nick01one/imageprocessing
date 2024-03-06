namespace ImageProcessingCore.Configuration;

public class TopicOptions
{
    public const string SectionKey = "Topics";
    public string ConnectionString { get; set; } = string.Empty;
    public string TopicName { get; set; } = string.Empty;
    public string SubscriptionName { get; set; } = string.Empty;
}
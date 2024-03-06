namespace ImageProcessingCore.Services;

public interface ITopicService
{
    Task SendMessageAsync(Guid taskId);
}
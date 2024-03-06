using Azure.Messaging.ServiceBus;
using ImageProcessingCore.Configuration;
using Microsoft.Extensions.Options;

namespace ImageProcessingCore.Services;

public class TopicService : ITopicService
{
    private readonly ServiceBusSender _sender;

    public TopicService(IOptions<TopicOptions> topicOptions)
    {
        var options = topicOptions.Value;
        var client = new ServiceBusClient(options.ConnectionString);
        _sender = client.CreateSender(options.TopicName);
    }
    
    public async Task SendMessageAsync(Guid taskId) =>
        await _sender.SendMessageAsync(new ServiceBusMessage(taskId.ToString()));
}
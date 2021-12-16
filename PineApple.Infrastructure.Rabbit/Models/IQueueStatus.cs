namespace PineApple.Infrastructure.Rabbit.Models
{
    public interface IQueueStatus
    {
        uint ConsumerCount { get; }
        uint MessageCount { get; }
        string QueueName { get; }
    }
}
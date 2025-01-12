using RabbitMQ.Contracts;
namespace Application.Core
{
    public interface IMessageBrokerService
    {
        Task Publish(EventType @event);
    }
}

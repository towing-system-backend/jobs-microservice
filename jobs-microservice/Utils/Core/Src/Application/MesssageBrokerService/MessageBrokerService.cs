using Application.Core;
using RabbitMQ.Contracts;

namespace jobs_microservice.Utils.Core.Src.Application.MesssageBrokerService
{
    public interface IMessageBrokerService
    {
        Task Publish(EventType @event);
    }
}

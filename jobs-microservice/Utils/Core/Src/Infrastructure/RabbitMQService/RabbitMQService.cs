using Application.Core;
using MassTransit;
using Newtonsoft.Json;
using RabbitMQ.Contracts;

namespace Application.Core
{
    public class RabbitMQService(IPublishEndpoint publishEndponint) : IMessageBrokerService
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndponint;

        public async Task Publish(EventType @event)
        {
            var eventType = new EventType(
                @event.PublisherId,
                @event.Type,
                JsonConvert.SerializeObject(@event.Context),
                @event.OcurredDate
            );
            
            await _publishEndpoint.Publish(eventType);         
        }
    }
}

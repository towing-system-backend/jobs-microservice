using MassTransit;
using RabbitMQ.Contracts;
using System.Text.Json;

namespace Application.Core
{
    public class OrderStatusUpdatedConsumer(IServiceProvider serviceProvider) : IConsumer<EventType>
    {
        private readonly IServiceProvider _serviceProvider =  serviceProvider;

        public Task Consume(ConsumeContext<EventType> context)
        {
            if (context.Message.Type == "OrderStatusUpdated")
            {
                var orderStatus = JsonSerializer.Deserialize<UpdateOrder>(
                    context.Message.Context.ToString()!
                );

                if (orderStatus != null)
                {
                    var message = new UpdateOrder(
                        context.Message.PublisherId,
                        orderStatus.Status
                    );
                    new MessageProcessor(_serviceProvider).ProcessMessage(message);
                }
            }
            return Task.CompletedTask;
        }
    }
}

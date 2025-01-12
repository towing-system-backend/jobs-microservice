using MassTransit;
using RabbitMQ.Contracts;

namespace Application.Core
{
    public class EventOrderToAcceptConsumer(IServiceProvider serviceProvider) : IConsumer<EventOrderToAccept>
    {
        private readonly IServiceProvider _serviceProvider =  serviceProvider;
        public Task Consume(ConsumeContext<EventOrderToAccept> context)
        {

            var message = new EventOrderToAccept(
                context.Message.OrderId,
                context.Message.TowDriverId,
                context.Message.DeviceToken,
                context.Message.UpdatedAt
            );

            new MessageProcessor(_serviceProvider).ProcessMessage(message);
            return Task.CompletedTask;
        }
    }
}

using Hangfire;
using Jobs.Application;
using RabbitMQ.Contracts;

namespace Application.Core
{
    public class JobService
    (
        IMessageBrokerService messsageBrokerService,
        IOrderRepository orderRepository
    ) : IJobService
    {
        private readonly IMessageBrokerService _messsageBrokerService = messsageBrokerService;
        private readonly IOrderRepository _orderRepository = orderRepository;

        public void SetTimerForNotificationSent(string orderId)
        {
            Hangfire.BackgroundJob.Schedule(
                () => DriverRejected(orderId),
                TimeSpan.FromMinutes(6)
            );
        }

        public async Task DriverRejected(string orderId)
        {
            var statusChanged = await _orderRepository.FindOrderById( orderId );
            if (statusChanged.Equals("ToAccept"))
            {
                var message = new TowDriverResponse(orderId, "Rejected");
                await _messsageBrokerService.Publish(
                    new EventType(
                        orderId,
                        "TowDriverResponse",
                        message,
                        DateTime.UtcNow
                    )
                );
            }
            return;
        }
    }
}

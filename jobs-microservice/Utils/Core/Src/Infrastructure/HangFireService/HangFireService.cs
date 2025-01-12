using Hangfire;
using RabbitMQ.Contracts;
using System.Collections.Concurrent;
using Application.Core;

namespace Application.Core
{
    public class JobService(IMessageBrokerService messsageBrokerService) : IJobService
    {
        private readonly IMessageBrokerService _messsageBrokerService = messsageBrokerService;

        public void SetTimerForNotificationSent(string orderId)
        {
            Hangfire.BackgroundJob.Schedule(
                () => DriverRejected(orderId),
                TimeSpan.FromMinutes(1)
            );
        }

        public async Task DriverRejected(string orderId)
        {
            var message = new TowDriverResponse(orderId, "Rejected");

            await _messsageBrokerService.Publish(
                new EventType(
                    orderId,
                    "TowDriverResponse",
                    message,
                    DateTime.UtcNow
                ));
        }
    }
}

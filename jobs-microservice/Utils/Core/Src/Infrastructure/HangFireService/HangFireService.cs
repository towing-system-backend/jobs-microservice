using Hangfire;
using jobs_microservice.Src.Infrastructure.Controllers.Dtos;
using jobs_microservice.Utils.Core.Src.Application.JobService;
using jobs_microservice.Utils.Core.Src.Application.MesssageBrokerService;
using RabbitMQ.Contracts;
using System.Collections.Concurrent;
using System.Text.Json;

namespace jobs_microservice.Utils.Core.Src.Infrastructure.HangFireService
{
    public class JobService(IMessageBrokerService messsageBrokerService) : IJobService
    {
        private readonly IMessageBrokerService _messsageBrokerService = messsageBrokerService;
        private readonly ConcurrentDictionary<string, string> _jobIds = new();


        public void ProcessOrderStatus(UpdateOrderDto dto)
        {
            switch (dto.Status.ToLower())
            {
                case "toaccept":
                    var jobId = BackgroundJob.Schedule(
                        () => HandlePendingOrder(dto),
                        TimeSpan.FromMinutes(1)
                    );
                    _jobIds.TryAdd(dto.Id, jobId);
                    break;

                case "cancelled":
                case "accepted":
                    // Si existe un job pendiente para esta orden, lo cancelamos
                    if (_jobIds.TryRemove(dto.Id, out var existingJobId))
                    {
                        BackgroundJob.Delete(existingJobId);
                    }

                    if (dto.Status.ToLower() == "cancelled")
                    {
                        BackgroundJob.Enqueue(() => HandleCancelledOrder(dto));
                    }
                    else
                    {
                        BackgroundJob.Enqueue(() => HandleAcceptedOrder(dto));
                    }
                    break;
            }
        }

        public async Task HandlePendingOrder(UpdateOrderDto dto)
        {
            _jobIds.TryRemove(dto.Id, out _);

            Console.WriteLine($"Order {dto.Id} not accepted after 6 minutes");
            var message = new TowDriverResponse(
                dto.Id,
                "Rejected"
            );
            await _messsageBrokerService.Publish(new EventType(
                dto.Id,
                "TowDriverResponse",
                message,
                DateTime.UtcNow
            ));
        }

        public void HandleCancelledOrder(UpdateOrderDto dto)
        {
            Console.WriteLine($"Processing cancelled order {dto.Id}");
        }

        public void HandleAcceptedOrder(UpdateOrderDto dto)
        {
            Console.WriteLine($"Processing accepted order {dto.Id}");
        }
    }
}

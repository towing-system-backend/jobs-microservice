using Application.Core;
using Jobs.Application;
using Microsoft.AspNetCore.Mvc;
using Order.Infrastructure;

namespace Job.Infrastructure 
{

    [ApiController]
    [Route("api/jobs")]
    public class JobController
    (
        IJobService jobService,
        INotificationService notificationService,
        IMessageBrokerService messageBrokerService
    ) : ControllerBase
    {
        private readonly IJobService _jobService = jobService;
        private readonly INotificationService _notificationService = notificationService;
        private readonly IMessageBrokerService _messageBrokerService = messageBrokerService;    

        [HttpPost("notification/sent")]
        public async Task<ObjectResult> EventOrderToAccept(OrderToAcceptDto dto)
        {
            var data = new NotifyDriverCommand(dto.Id, dto.DevideToken);
            var handler = new NotifyDriverHandler(_jobService, _notificationService);
            var res = await handler.Execute(data);
            return Ok(res);
        }
    }
}

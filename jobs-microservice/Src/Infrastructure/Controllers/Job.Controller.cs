using Hangfire;
using jobs_microservice.Src.Infrastructure.Controllers.Dtos;
using jobs_microservice.Utils.Core.Src.Application.JobService;
using jobs_microservice.Utils.Core.Src.Application.MesssageBrokerService;
using Microsoft.AspNetCore.Mvc;

namespace Job.Infrastructure 
{

    [ApiController]
    [Route("api/jobs")]
    public class JobController
    (
        IJobService jobService,
        IMessageBrokerService messageBrokerService
    ) : ControllerBase
    {
        private readonly IJobService _jobService = jobService;
        private readonly IMessageBrokerService _messageBrokerService = messageBrokerService;    

        [HttpPost("Notification/Sent")]
        public IActionResult UpdateOrder(UpdateOrderDto dto)
        {
           _jobService.ProcessOrderStatus(dto);
            return Ok("Order status processed");
        }
    }

}

using jobs_microservice.Src.Infrastructure.Controllers.Dtos;

namespace jobs_microservice.Utils.Core.Src.Application.JobService
{
    public interface IJobService
    {
        void ProcessOrderStatus(UpdateOrderDto dto);
    }
}


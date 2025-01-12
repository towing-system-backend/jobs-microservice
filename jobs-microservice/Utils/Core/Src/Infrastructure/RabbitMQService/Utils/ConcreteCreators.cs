using jobs_microservice.Src.Infrastructure.Controllers.Dtos;

namespace RabbitMQ.Contracts
{
    public class UpdateOrderDtoCreator : DtoCreator<UpdateOrder, UpdateOrderDto>
    {
        public override UpdateOrderDto CreateDto(UpdateOrder message)
        {
            return new UpdateOrderDto(
                message.PublisherId,
                message.Status
            );
        }
    }
}

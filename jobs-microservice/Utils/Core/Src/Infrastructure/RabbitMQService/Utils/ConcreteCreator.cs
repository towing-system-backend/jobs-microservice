using Job.Infrastructure;

namespace RabbitMQ.Contracts
{
    public class EventOrderToAcceptDtoCreator : DtoCreator<EventOrderToAccept, OrderToAcceptDto>
    {
        public override OrderToAcceptDto CreateDto(EventOrderToAccept message)
        {
            return new OrderToAcceptDto(
                message.OrderId,
                message.TowDriverId,
                message.DeviceToken
            );
        }
    }
}

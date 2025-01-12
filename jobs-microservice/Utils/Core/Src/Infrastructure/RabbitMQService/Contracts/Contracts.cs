namespace RabbitMQ.Contracts
{
    public interface IRabbitMQMessage { };

    public record EventType(
        string PublisherId,
        string Type,
        object Context,
        DateTime OcurredDate
    );

    public record EventOrderToAccept(
        Guid OrderId,
        string? TowDriverId,
        string? DeviceToken,
        DateTime UpdatedAt
    ) : IRabbitMQMessage;

    public record TowDriverResponse(
        string OrderId,
        string Status
    ) : IRabbitMQMessage;
}


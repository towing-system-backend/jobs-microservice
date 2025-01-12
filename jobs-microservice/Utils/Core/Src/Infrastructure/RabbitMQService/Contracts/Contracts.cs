namespace RabbitMQ.Contracts
{
    public interface IRabbitMQMessage { };

    public record EventType(
        string PublisherId,
        string Type,
        object Context,
        DateTime OcurredDate
    );

    public record UpdateOrder(
        string PublisherId,
        string Status
    ) : IRabbitMQMessage;

    public record TowDriverResponse(
        string OrderId,
        string Status
    ) : IRabbitMQMessage;
}


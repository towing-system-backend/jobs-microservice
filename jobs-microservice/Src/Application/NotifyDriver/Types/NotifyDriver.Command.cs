namespace Jobs.Application
{
    public record NotifyDriverCommand(Guid OrderId, string DeviceToken);
    
}

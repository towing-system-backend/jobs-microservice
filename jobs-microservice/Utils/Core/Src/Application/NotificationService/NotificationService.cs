namespace Application.Core 
{
    public interface INotificationService
    {
        Task SendNotification(string deviceToken, string orderId);
    }
}



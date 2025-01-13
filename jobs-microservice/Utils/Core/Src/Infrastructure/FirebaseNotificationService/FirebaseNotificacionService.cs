using FirebaseAdmin.Messaging;

namespace Application.Core
{
    public class FirebaseNotificationsService : INotificationService
    {
        public async Task SendNotification(string deviceToken, string orderId)
        {
            var message = new Message
            {
                Token = deviceToken,
                Notification = new Notification
                {
                    Title = "Nueva solicitud de servicio",
                    Body = "¿Deseas aceptar o rechazar esta solicitud?"
                },
                Data = new Dictionary<string, string>
                {
                    { "order_id", orderId },
                    { "category", "order" }, 
                    { "click_action", "FLUTTER_NOTIFICATION_CLICK" }
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        Title = "Nueva solicitud de servicio",
                        Body = "¿Deseas aceptar o rechazar esta solicitud?",
                        ChannelId = "default",
                        ClickAction = "FLUTTER_NOTIFICATION_CLICK"
                    }
                }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

    }
}
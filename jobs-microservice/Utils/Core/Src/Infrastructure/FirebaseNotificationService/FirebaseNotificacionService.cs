using FirebaseAdmin.Messaging;

namespace Application.Core
{
    public class FirebaseNotificationsService : INotificationService
    {

        public async Task SendNotification(string deviceToken, string orderId)
        {
            Console.WriteLine($"deviceToken: {deviceToken}");
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
                    { "action_accept", "Aceptar" },
                    { "action_reject", "Rechazar" }
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        Title = "Nueva solicitud de servicio",
                        Body = "¿Deseas aceptar o rechazar esta solicitud?",
                        ChannelId = "default_channel"
                    }
                }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

    }
}
using Application.Core;

namespace Jobs.Application 
{
    public class NotifyDriverHandler
    (
        IJobService jobService,
        INotificationService notificationService
    ) : IService<NotifyDriverCommand, NotifyDriverResponse>
    {
        private readonly IJobService _jobService = jobService;
        private readonly INotificationService _notificationService = notificationService;
        public async Task<Result<NotifyDriverResponse>> Execute(NotifyDriverCommand data)
        {
            await _notificationService.SendNotification(data.DeviceToken, data.OrderId.ToString());
            _jobService.SetTimerForNotificationSent(data.OrderId.ToString());
            return Result<NotifyDriverResponse>.MakeSuccess(new NotifyDriverResponse());
        }
    }
}



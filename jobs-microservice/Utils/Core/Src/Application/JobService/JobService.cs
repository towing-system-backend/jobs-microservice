using Order.Infrastructure;

namespace Application.Core
{
    public interface IJobService
    {
        void SetTimerForNotificationSent(string orderId);
    }
}


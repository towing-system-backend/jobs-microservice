namespace Application.Core
{
    public interface IOrderRepository
    {
        Task<string> FindOrderById(string  orderId);
    }
}

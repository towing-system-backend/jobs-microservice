using Job.Infrastructure;
using MongoDB.Driver;

namespace Application.Core
{
    public class MongoOrderServices : IOrderRepository
    {
        private readonly IMongoCollection<MongoOrder> _orderCollection;
        public MongoOrderServices()
        {
            var client = new MongoClient(Environment.GetEnvironmentVariable("CONNECTION_URI"));
            var database = client.GetDatabase(Environment.GetEnvironmentVariable("DATABASE_NAME"));
            _orderCollection = database.GetCollection<MongoOrder>("orders");
        }

        public async Task<string> FindOrderById(string orderId)
        {
            var filter = Builders<MongoOrder>.Filter.Eq(order => order.OrderId, orderId);
            var res = await _orderCollection.Find(filter).FirstOrDefaultAsync();

            return res.Status;
        }
    }
}

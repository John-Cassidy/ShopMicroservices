using Shopping.Aggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class OrderService : IOrderService {
        private readonly HttpClient _client;

        public OrderService(HttpClient client) => _client = client ?? throw new ArgumentNullException(nameof(client));

        public Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName) => throw new NotImplementedException();
    }
}

using FastFoodBackend.Contracts.BrokerModels;
using FastFoodBackend.OrdersAssembly.Application.Abstract.Services;

namespace FastFoodBackend.OrdersAssembly.Application.Services
{
    public class OrdersTrackerService : IOrdersTrackerService
    {
        public event EventHandler<OrderCompleted>? OrderIsReady;

        public Task OnPreparedItemReceived(ItemPrepared preparedItem)
        {
            throw new NotImplementedException();
        }
    }
}

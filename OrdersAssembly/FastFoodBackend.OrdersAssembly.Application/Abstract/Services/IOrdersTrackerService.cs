using FastFoodBackend.Contracts.BrokerModels;

namespace FastFoodBackend.OrdersAssembly.Application.Abstract.Services
{
    public interface IOrdersTrackerService
    {
        Task OnPreparedItemReceived(ItemPrepared preparedItem);

        event EventHandler<OrderCompleted> OrderIsReady; 
    }
}

using FastFoodBackend.Contracts.BrokerModels;

namespace FastFoodBackend.OrdersAssembly.Application.Abstract.Services
{
    public interface IOrdersTrackerService : IDisposable
    {
        Task OnPreparedItemReceived(ItemPrepared preparedItem);

        event Func<object, OrderCompleted, Task>? OrderIsReadyAsync;
    }
}

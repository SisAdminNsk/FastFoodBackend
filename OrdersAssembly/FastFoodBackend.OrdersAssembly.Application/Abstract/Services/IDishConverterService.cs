using FastFoodBackend.Contracts.ApiModels;

namespace FastFoodBackend.OrdersAssembly.Application.Abstract.Services
{
    public interface IDishConverterService
    {
        IOrderItem ConvertDish(DishType dishType, object serializedDish); 
    }
}

using FastFoodBackend.Contracts.ApiModels;
using FastFoodBackend.Contracts.ReflectionUtils;
using FastFoodBackend.OrdersAssembly.Application.Abstract.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FastFoodBackend.OrdersAssembly.Application.Services
{
    public class DishConverterService : IDishConverterService
    {
        private readonly ILogger<IDishConverterService> _logger;
        public DishConverterService(ILogger<IDishConverterService> logger)
        {
            _logger = logger;
        }

        public IOrderItem ConvertDish(DishType dishType, object serializedDish)
        {
            var dishTypeClass = GetDishTypeClassOrThrowIfTypeNotAssigned(dishType);

            return DeserializeDishOrThrow(serializedDish.ToString(), dishTypeClass);
        }
        private Type GetDishTypeClassOrThrowIfTypeNotAssigned(DishType dishType)
        {
            if (!DishTypeToDishClassMapper.Map.TryGetValue(dishType, out Type dishTypeValue))
            {
                throw new NotSupportedException($"Тип блюда {dishType} не зарегистрирован в системе");
            }

            return dishTypeValue;
        }
        private IOrderItem DeserializeDishOrThrow(string serializedDish, Type dishType)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                    PropertyNameCaseInsensitive = true
                };

                var dish = JsonSerializer.Deserialize(serializedDish, dishType, options);

                if (dish is IOrderItem orderItem)
                {
                    return orderItem;
                }

                throw new InvalidOperationException($"Объект типа {dishType.Name} не реализует IOrderItem");
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Ошибка десериализации для типа {dishType}", ex);
            }
        }
    }
}

using FastFoodBackend.Contracts.ApiModels;
using System.Reflection;

namespace FastFoodBackend.Contracts.ReflectionUtils
{
    public static class DishTypeToDishClassMapper
    {
        public static Dictionary<DishType, Type> Map { get; private set; }  = Initialize();
        private static Dictionary<Type, DishType> ReverseMap = InitializeReverseMap();
        private static Dictionary<DishType, Type> Initialize()
        {
            var map = new Dictionary<DishType, Type>();

            // Получаем все значения enum DishType
            var dishTypes = Enum.GetValues(typeof(DishType)).Cast<DishType>();

            // Получаем все типы в текущей сборке, которые реализуют IOrderItem
            var orderItemTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IOrderItem).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            // Сопоставляем DishType с классами по имени
            foreach (var dishType in dishTypes)
            {
                // Ищем тип, имя которого совпадает с именем DishType
                var matchingType = orderItemTypes.FirstOrDefault(t =>
                    t.Name.Equals(dishType.ToString(), StringComparison.OrdinalIgnoreCase));

                if (matchingType != null)
                {
                    map[dishType] = matchingType;
                }
            }

            return map;
        }
        private static Dictionary<Type, DishType> InitializeReverseMap()
        {
            var map = new Dictionary<Type, DishType>();

            foreach(var keyVal in Map)
            {
                var dishClass = keyVal.Key;
                var dishType = keyVal.Value;

                map[dishType] = dishClass;
            }

            return map;
        }
        public static DishType GetDishTypeByDishClass(Type dishClass)
        {
            if (ReverseMap.TryGetValue(dishClass, out var dishType))
            {
                return dishType;
            }

            throw new KeyNotFoundException($"Тип {dishClass} не зарегистрирован в ReverseMap");
        }
        public static void RegistrateNewType<Dish>(DishType dishType, Dish dish)
            where Dish : IOrderItem
        {
            if (Map.ContainsKey(dishType))
            {
                throw new ArgumentException($"Тип {dishType} уже зарегистрирован");
            }

            Map[dishType] = typeof(Dish);
        }

        // поменял Master

        // Тестовый коммит в master #1

        // Тестовый коммит в master #2

        // Тестовый коммит в master #3 с конфликтом

        // Поменял чтобы был конфликт

        // Мастер отдалился на 1 коммит

        // Мастер отдалился на 2 коммит
    }
}

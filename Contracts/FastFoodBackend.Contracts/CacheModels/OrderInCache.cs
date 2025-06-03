namespace FastFoodBackend.Contracts.CacheModels
{
    public class OrderInCache
    {
        public Guid OrderId { get; set; }
        public int TotalItemsInOrder { get; set; }
        public int TotalCookedItems { get; set; }
        public Dictionary<string, bool> ItemsInOrderStatuses { get; set; } = new();

        public OrderInCache(Guid orderId, int totalItemsInOrder, int totalCookedItems, Dictionary<string, bool> itemsInOrderStatuses)
        {
            OrderId = orderId;
            TotalCookedItems = totalCookedItems;
            TotalItemsInOrder = totalItemsInOrder;
            ItemsInOrderStatuses = itemsInOrderStatuses;
        }
    }
}

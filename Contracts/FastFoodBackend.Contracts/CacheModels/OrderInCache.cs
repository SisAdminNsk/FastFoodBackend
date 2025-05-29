namespace FastFoodBackend.Contracts.CacheModels
{
    public class OrderInCache
    {
        public int TotalItemsInOrder { get; set; }
        public int TotalCookedItems { get; set; }
        public Dictionary<string, bool> ItemsInOrderStatuses { get; set; } = new();

        public OrderInCache(int totalItemsInOrder, int totalCookedItems, Dictionary<string, bool> itemsInOrderStatuses)
        {
            TotalCookedItems = totalCookedItems;
            TotalItemsInOrder = totalItemsInOrder;
            ItemsInOrderStatuses = itemsInOrderStatuses;

            InitializeAfterAssign();
        }

        private void InitializeAfterAssign()
        {
            TotalCookedItems = 0;
            TotalCookedItems = ItemsInOrderStatuses.Count;

            foreach (var item in ItemsInOrderStatuses.Keys)
            {
                ItemsInOrderStatuses[item] = false;
            }
        }
    }
}

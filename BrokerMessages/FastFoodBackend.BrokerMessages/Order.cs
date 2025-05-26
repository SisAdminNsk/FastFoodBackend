namespace FastFoodBackend.BrokerMessages
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime AcceptionTime { get; set; }
        public DateTime? ReadyTime { get; set; }
        public List<Drink> Drinks { get; set; } = new();
        public List<HotDish> HotDishes { get; set; } = new();
        public List<ColdDish> ColdDishes { get; set; } = new();

        public int ItemsCount()
        {
            return Drinks.Count + HotDishes.Count + ColdDishes.Count;
        }
        public List<IOrderItem> GetAllItems()
        {
            var items = new List<IOrderItem>();

            foreach(var drink in Drinks)
            {
                items.Add(drink);
            }

            foreach(var hotDish in HotDishes)
            {
                items.Add(hotDish);
            }

            foreach(var coldDish in ColdDishes)
            {
                items.Add(coldDish);
            }

            return items;
        }
    }
}

using FastFoodBackend.Contracts.ApiModels;

namespace FastFoodBackend.Contracts.ApiAndBrokersModels
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

            foreach (var drink in Drinks)
            {
                items.Add(drink);
            }

            foreach (var hotDish in HotDishes)
            {
                items.Add(hotDish);
            }

            foreach (var coldDish in ColdDishes)
            {
                items.Add(coldDish);
            }

            return items;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine($"Order ID: {Id}");
            sb.AppendLine($"Accepted at: {AcceptionTime}");
            sb.AppendLine($"Ready at: {(ReadyTime.HasValue ? ReadyTime.ToString() : "not ready yet")}");
            sb.AppendLine($"Total items: {ItemsCount()}");

            sb.AppendLine("\nDrinks:");
            foreach (var drink in Drinks)
            {
                sb.AppendLine($"- {drink}");
            }

            sb.AppendLine("\nHot Dishes:");
            foreach (var hotDish in HotDishes)
            {
                sb.AppendLine($"- {hotDish}");
            }

            sb.AppendLine("\nCold Dishes:");
            foreach (var coldDish in ColdDishes)
            {
                sb.AppendLine($"- {coldDish}");
            }

            return sb.ToString();
        }
    }
}

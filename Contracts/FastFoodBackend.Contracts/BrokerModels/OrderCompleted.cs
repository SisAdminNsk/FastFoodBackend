namespace FastFoodBackend.Contracts.BrokerModels
{
    public class OrderCompleted
    {
        public DateTime CompletionTime { get; set; }
        public Guid OrderId { get; set; }
        public List<string> PreparedDishes { get; set; } = new ();
        public OrderCompleted(Guid orderId, List<string> preparedDishes)
        {
            OrderId = orderId;
            PreparedDishes = preparedDishes;

            CompletionTime = DateTime.UtcNow;
        }
        public OrderCompleted()
        {

        }

        public override string ToString()
        {
            var dishes = PreparedDishes.Count == 0
                ? "no dishes"
                : string.Join(", ", PreparedDishes);

            return $"Order {OrderId} completed at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC). Prepared dishes: {dishes}";
        }
    }
}

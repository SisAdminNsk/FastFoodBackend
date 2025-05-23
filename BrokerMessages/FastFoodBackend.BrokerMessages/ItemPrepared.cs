namespace FastFoodBackend.BrokerMessages
{
    public class ItemPrepared
    {
        public Guid OrderId { get; set; }
        public string Type { get; set; }
        public object Item { get; set; }

        public ItemPrepared(Guid orderId, string type, object item)
        {
            OrderId = orderId;
            Type = type;
            Item = item;
        }
    }
}

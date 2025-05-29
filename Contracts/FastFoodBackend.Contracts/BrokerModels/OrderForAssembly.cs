using FastFoodBackend.Contracts.ApiAndBrokersModels;

namespace FastFoodBackend.Contracts.BrokerModels
{
    public class OrderForAssembly
    {
        public Order Order { get; set; }
        public bool GiveWhenAllReady { get; set; }

        public OrderForAssembly(Order order, bool giveWhenAllReady = false)
        {
            Order = order;
            GiveWhenAllReady = giveWhenAllReady;
        }
    }
}

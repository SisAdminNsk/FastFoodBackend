using FastFoodBackend.Contracts.ApiModels;
using FastFoodBackend.Contracts.BrokerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodBackend.OrdersAssembly.Application.Abstract.Services
{
    public interface IDishConverter
    {
        IOrderItem ConvertDish(); 
    }
}

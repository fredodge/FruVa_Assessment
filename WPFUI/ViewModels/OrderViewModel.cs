using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPFUI.API;
using WPFUI.Models;

namespace WPFUI.ViewModels
{
    class OrderViewModel
    {
        private Logger Log;
        public OrderService ordersAPI;
        public OrderItemService orderItemsService;

        public OrderViewModel() {
            Log = new Logger();
            ordersAPI = new OrderService();
            orderItemsService = new OrderItemService();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await ordersAPI.GetOrdersAsync();
        }

        public async Task<bool> DeleteOrder(Order order)
        {
            (await orderItemsService.GetOrderItemsByOrderAsync(order.Id)).ForEach(async oi => await orderItemsService.DeleteOrderItem(oi.Id));
            return await ordersAPI.DeleteOrder(order.Id);
        }

        public void Dispose()
        {
            // Discpose APIS 
        }
    }
}

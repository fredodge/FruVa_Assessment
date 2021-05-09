using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPFUI.API;
using WPFUI.Models;

namespace WPFUI.ViewModels
{
    class EditOrderArticlesViewModel : IDisposable
    {
        private Logger Log;
        private ArticlesAPI articleAPI;
        private OrderItemsService orderItemsService;
        public Order orderEditObject { get; set; }
        public List<OrderItem> orderItemsEditList { get; set; }

        public EditOrderArticlesViewModel()
        {
        }

        public EditOrderArticlesViewModel(Order order)
        {
            orderEditObject = order;
        }

        public void Load()
        {
            Log = new Logger();
            articleAPI = new ArticlesAPI();
            orderItemsService = new OrderItemsService();
            if (orderEditObject == null) { 
                orderEditObject = new Order();
                orderEditObject.Id = Guid.NewGuid();
            }
            orderItemsEditList = new List<OrderItem>();
        }

        public async Task<List<Article>> GetArticlesAsync()
        {
            return await articleAPI.GetArticlesAsync();
        }

        public async Task<Article> GetArticleByIdAsync(Guid id)
        {
            return await articleAPI.GetArticleByIdAsync(id);
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderAsync(Order order)
        {
            return await orderItemsService.GetOrderItemsByOrderAsync(order.Id);
        }

        public void Dispose()
        {
            // Dispose API
        }
    }
}

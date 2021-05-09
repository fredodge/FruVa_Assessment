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
        private ArticleService articleAPI;
        private OrderItemService orderItemsService;
        public Order order { get; set; }
        public List<OrderItem> orderItems { get; set; }

        public EditOrderArticlesViewModel()
        {
            Log = new Logger();
            this.order = new Order();
            this.orderItems = new List<OrderItem>();
        }

        public EditOrderArticlesViewModel(Order order)
        {
            Log = new Logger();
            this.order = order;
            this.orderItems = new List<OrderItem>();
        }

        public EditOrderArticlesViewModel(Order order, List<OrderItem> orderItems)
        {
            Log = new Logger();
            this.order = order;
            this.orderItems = orderItems;
        }

        public void Load()
        {
            articleAPI = new ArticleService();
            orderItemsService = new OrderItemService();
            if (order == null) {
                order = new Order();
                order.Id = Guid.NewGuid();
            }
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

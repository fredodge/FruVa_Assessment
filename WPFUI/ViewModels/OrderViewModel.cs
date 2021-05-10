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
        public RecipientService recipientService;
        public ArticleService articleService;
        public List<OrderViewItem> orderviewItems { get; set; }

        public OrderViewModel() {
            Log = new Logger();
            ordersAPI = new OrderService();
            orderItemsService = new OrderItemService();
            recipientService = new RecipientService();
            articleService = new ArticleService();
            orderviewItems = new List<OrderViewItem>();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await ordersAPI.GetOrdersAsync();
        }

        public async Task<bool> DeleteOrder(Guid orderId)
        {
            (await orderItemsService.GetOrderItemsByOrderAsync(orderId)).ForEach(async oi => await orderItemsService.DeleteOrderItem(oi.Id));
            return await ordersAPI.DeleteOrder(orderId);
        }

        public async Task<Recipient> GetRecipientByIdAsync(Guid recipientId)
        {
            return await recipientService.GetRecipientByIdAsync(recipientId);
        }

        public async Task<Article> GetArticleByIdAsync(Guid articleId)
        {
            return await articleService.GetArticleByIdAsync(articleId);
        }

        public void Dispose()
        {
            // Discpose APIS 
        }
    }
}

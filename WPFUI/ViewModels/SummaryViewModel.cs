using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPFUI.API;
using WPFUI.Models;

namespace WPFUI.ViewModels
{
    class SummaryViewModel
    {
        private Logger Log;
        public OrderService ordersService;
        public OrderItemService orderItemsService;
        public RecipientService recipientService;
        public ArticleService articleService;
        public Order order { get; set; }
        public List<OrderItem> orderItems { get; set; }

        public SummaryViewModel()
        {
            Log = new Logger();
        }

        public SummaryViewModel(Order order, List<OrderItem> orderItems)
        {
            Log = new Logger();
            this.order = order;
            this.orderItems = orderItems;
        }

        public void Load()
        {
            ordersService = new OrderService();
            orderItemsService = new OrderItemService();
            recipientService = new RecipientService();
            articleService = new ArticleService();
        }

        public async Task<Recipient> GetRecipientByIdAsync(Guid recipientId)
        {
            return await recipientService.GetRecipientByIdAsync(recipientId);
        }
        public async Task<Article> GetArticleByIdAsync(Guid id)
        {
            return await articleService.GetArticleByIdAsync(id);
        }

        public void Dispose()
        {
            // Discpose APIS 
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel;
using WPFUI.API;
using WPFUI.Models;
using System.Collections.ObjectModel;

namespace WPFUI.ViewModels
{
    class EditOrderViewModel : IDisposable
    {
        private Logger Log;
        private ArticleService articleAPI;
        private RecipientService recipientAPI;
        private OrderService ordersAPI;
        private OrderItemService orderItemsService;
        private Order orderEditObject;

        public EditOrderViewModel()
        {
            Log = new Logger();
        }

        public EditOrderViewModel(Order order)
        {
            Log = new Logger();
            orderEditObject = order;
            Log.Log($"{orderEditObject.OrderName}");
        }

        public void Load()
        {
            articleAPI = new ArticleService();
            recipientAPI = new RecipientService();
            ordersAPI = new OrderService();
            orderItemsService = new OrderItemService();
            Log.Log("API loaded.");
        }

        public async Task<List<Recipient>> GetRecipientsAsync()
        {
            return await recipientAPI.GetRecipientsAsync();
        }

        public async Task<List<Article>> GetArticlesAsync()
        {
            return await articleAPI.GetArticlesAsync();
        }

        public async Task<Article> GetArticleByIdAsync(Guid id)
        {
            return await articleAPI.GetArticleByIdAsync(id);
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await ordersAPI.GetOrdersAsync();
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderAsync(Order order)
        {
            return await orderItemsService.GetOrderItemsByOrderAsync(order.Id);
        }

        public void EditOrder(List<Article> Articles, Guid OrderId)
        {
            /*
            using (var Context = new FruVa_Assessment_OrdersEntities())
            {
                Context.Database.Connection.Open();

                foreach (var OrderItem in Context.OrderItems)
                {
                    if (OrderItem.OrderId == OrderId)
                    {
                        Context.OrderItems.Remove(OrderItem);
                    }
                }

                foreach (var Article in Articles)
                {
                    var OrderItem = new OrderItems();
                    OrderItem.Id = Guid.NewGuid();
                    OrderItem.ArticleId = Article.Id;
                    OrderItem.Amount = 1;
                    OrderItem.OrderId = OrderId;
                    Context.OrderItems.Add(OrderItem);
                }
                Context.SaveChanges();

                Context.Database.Connection.Close();
            }*/
        }

        public void Dispose()
        {
            // Dispose API
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel;
using WPFUI.Models;
using WPFUI.API;

namespace WPFUI.ViewModels
{
    class CreateOrderViewModel : IDisposable
    {
        private Logger Log;
        private ArticleService articleAPI;
        private RecipientService recipientAPI;
        private OrderService ordersAPI;
        private OrderItemService orderItemsService; 

        public CreateOrderViewModel()
        {
            Log = new Logger();
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

        public async Task PostOrderItemsAsync(List<OrderItem> orderItems)
        {
            orderItems.ForEach(async oi => await orderItemsService.PostOrderItem(oi));
        } 

        public async Task<Order> CreateOrder(List<Article> Articles, Recipient Recipients, string OrderName) 
        {
            var Order = new Order();
            Order.DeliveryDay = BitConverter.GetBytes(DateTime.Now.Ticks);
            Order.OrderName = OrderName + "AtAPI";
            Order.RecipientId = Recipients.Id;

            var OrderPlaced = (await ordersAPI.PostOrder(Order));

            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var Article in Articles)
            {
                var OrderItem = new OrderItem();
                OrderItem.ArticleId = Article.Id;
                OrderItem.Amount = 1;
                OrderItem.OrderId = OrderPlaced.Id;
                orderItems.Add(OrderItem);
            }

            await PostOrderItemsAsync(orderItems);

            Order.OrderItems = orderItems;
            return Order;
            // ordersAPI UPDATE
            /*
            using (var Context = new FruVa_Assessment_OrdersEntities())
            {
                Context.Database.Connection.Open();

                foreach (var Article in Articles)
                {
                    var OrderItem = new OrderItems();
                    OrderItem.ArticleId = Article.Id;
                    OrderItem.Amount = 1;
                    OrderItem.OrderId = OrderPlaced.Id;
                    Context.OrderItems.Add(OrderItem);
                }
                Context.SaveChanges();
                    
                Context.Database.Connection.Close();
            }
            */
        }

        public void Dispose()
        {
            // Dispose API
        }
    }
}

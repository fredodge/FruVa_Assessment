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
        public ArticlesAPI articleAPI;
        public RecipientAPI recipientAPI;

        public CreateOrderViewModel()
        {
            Log = new Logger();
        }

        public void Load()
        {
            articleAPI = new ArticlesAPI();
            recipientAPI = new RecipientAPI();
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

        public void CreateOrder(List<Articles> Articles, Recipients Recipients, string OrderName) 
        {
            using (var Context = new FruVa_Assessment_OrdersEntities())
            {
                Context.Database.Connection.Open();

                var Order = new Orders();
                Order.Id = Guid.NewGuid();
                Order.DeliveryDay = BitConverter.GetBytes(DateTime.Now.Ticks);
                Order.OrderName = OrderName;
                Order.RecipientId = Recipients.Id;
                Log.Log($"{Order.Id}");
                var OrderPlaced = Context.Orders.Add(Order);
                Log.Log($"{OrderPlaced.Id}");
                Context.SaveChanges();

                foreach(var Article in Articles)
                {
                    var OrderItem = new OrderItems();
                    OrderItem.Id = Guid.NewGuid();
                    OrderItem.ArticleId = Article.Id;
                    OrderItem.Amount = 1;
                    OrderItem.OrderId = OrderPlaced.Id;
                    Context.OrderItems.Add(OrderItem);
                }
                Context.SaveChanges();
                    
                Context.Database.Connection.Close();
            }
        }

        public void Dispose()
        {
            using (var context = new FruVa_Assessment_APIEntities()) { context.Dispose(); }
        }
    }
}

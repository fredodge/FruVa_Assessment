using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel;

namespace WPFUI.ViewModels
{
    class CreateOrderViewModel : IDisposable
    {
        private Logger Log;
        public List<Articles> articles_context;
        public List<Recipients> recipients;

        public CreateOrderViewModel()
        {
            Log = new Logger();
            articles_context = new List<Articles>();
            recipients = new List<Recipients>();
        }

        public void Load()
        {
            using (var context = new FruVa_Assessment_APIEntities())
            {
                context.Database.Connection.Open();

                foreach ( var article in context.Articles)
                {
                    articles_context.Add(article);
                }

                foreach ( var recipient in context.Recipients)
                {
                    recipients.Add(recipient);
                }

                context.Database.Connection.Close();
            }
            Log.Log("Articles & Recipients loaded.");
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

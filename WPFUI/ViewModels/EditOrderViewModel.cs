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
    class EditOrderViewModel : IDisposable
    {
        private Logger Log;
        public List<Articles> articles_context;
        public List<Recipients> recipients;
        public Orders Order;

        public EditOrderViewModel()
        {
            Log = new Logger();
            articles_context = new List<Articles>();
        }
        public EditOrderViewModel(Orders Order)
        {
            this.Order = Order;
        }

        public void Load()
        {
            Log = new Logger();
            articles_context = new List<Articles>();
            this.recipients = new List<Recipients>();

            using (var context = new FruVa_Assessment_APIEntities())
            {
                context.Database.Connection.Open();

                foreach (var article in context.Articles)
                {
                    articles_context.Add(article);
                }

                foreach (var recipient in context.Recipients)
                {
                    recipients.Add(recipient);
                }

                context.Database.Connection.Close();
            }
            Log.Log("Articles loaded.");
        }

        public void EditOrder(List<Articles> Articles, Guid OrderId)
        {
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
            }
        }

        public void Dispose()
        {
            using (var context = new FruVa_Assessment_APIEntities()) { context.Dispose(); }
        }
    }
}

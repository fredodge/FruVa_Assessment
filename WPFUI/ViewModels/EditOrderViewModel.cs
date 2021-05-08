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
        public ArticlesAPI articleAPI;
        public RecipientAPI recipientAPI;
        public Orders Order;

        public EditOrderViewModel()
        {
            Log = new Logger();
        }
        public EditOrderViewModel(Orders Order)
        {
            this.Order = Order;
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

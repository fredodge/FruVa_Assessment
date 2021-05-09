﻿using System;
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
    class EditOrderRecipientViewModel : IDisposable
    {
        private Logger Log;
        private RecipientAPI recipientAPI;
        private OrdersAPI ordersAPI;
        private Order order;
        private List<OrderItem> orderItems;

        public EditOrderRecipientViewModel()
        {
            Log = new Logger();
        }

        public EditOrderRecipientViewModel(Order order)
        {
            Log = new Logger();
            this.order = order;
        }

        public EditOrderRecipientViewModel(Order order, List<OrderItem> orderItems)
        {
            Log = new Logger();
            this.order = order;
            this.orderItems = orderItems;
        }

        public void Load()
        {
            recipientAPI = new RecipientAPI();
            ordersAPI = new OrdersAPI();
            Log.Log("API loaded.");
        }

        public async Task<List<Recipient>> GetRecipientsAsync()
        {
            return await recipientAPI.GetRecipientsAsync();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await ordersAPI.GetOrdersAsync();
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

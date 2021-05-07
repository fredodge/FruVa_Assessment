using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUI.Views;

namespace WPFUI.ViewModels
{
    class MainViewModel
    {
        private Logger Log;
        public List<Orders> Orders;
        public List<OrderItems> OrderItems;

        public MainViewModel()
        {
            Log = new Logger();
        }

        public void Load()
        {
            Orders = new List<Orders>();
            OrderItems = new List<OrderItems>();
            using (var context = new FruVa_Assessment_OrdersEntities())
            {
                context.Database.Connection.Open();

                foreach (var order in context.Orders)
                {
                    Orders.Add(order);
                }

                foreach (var orderItems in context.OrderItems)
                {
                    OrderItems.Add(orderItems);
                }

                context.Database.Connection.Close();
            }
            Log.Log("Articles loaded.");
        }

        public void DeleteOrder(Orders Order)
        {
            using (var context = new FruVa_Assessment_OrdersEntities())
            {
                context.Database.Connection.Open();
                
                foreach (var orderItems in context.OrderItems)
                {
                    if (orderItems.Id == Order.Id)
                    {
                        context.OrderItems.Remove(orderItems);
                    }
                }
                context.SaveChanges();

                var set = context.Set<Orders>();
                foreach (Orders order in context.Orders)
                {
                    if (order.Id == Order.Id)
                    {
                        Log.Log($"Deleting {order.Id}");
                        set.Remove(order);
                        break;
                    }
                }
                context.SaveChanges();

                context.Database.Connection.Close();
            }
        }

        public void Dispose()
        {
            using (var context = new FruVa_Assessment_APIEntities()) { context.Dispose(); }
        }
    }
}

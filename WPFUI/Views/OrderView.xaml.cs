using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Models;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class OrderView : UserControl
    {
        Logger Log;
        OrderViewModel vm;
        public OrderView()
        {
            InitializeComponent();
            UserControl_Loaded(null, null);
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                Log = new Logger();
                vm = new OrderViewModel();

                (await vm.GetOrdersAsync()).ForEach(async order => {
                    var newItem = new OrderViewItem();
                    newItem.Id = order.Id;
                    newItem.OrderName = order.OrderName;
                    newItem.RecipientName = (await vm.GetRecipientByIdAsync(order.RecipientId)).Name;
                    newItem.ArticleAmount = 0;
                    newItem.ArticleNames = "";

                    /*
                    long longVar = BitConverter.ToInt64(order.DeliveryDay, 0);
                    newItem.DeliveryDay = new DateTime(1980, 1, 1).AddMilliseconds(longVar);
                    */
                    (await vm.orderItemsService.GetOrderItemsByOrderAsync(order.Id)).ForEach(async oi =>
                    {
                        newItem.ArticleAmount += oi.Amount;
                        newItem.ArticleNames += $"{(await vm.GetArticleByIdAsync(oi.ArticleId)).ArticleName}, ";
                    });

                    vm.orderviewItems.Add(newItem);
                    DatagridXAML.Items.Add(newItem);
                });
                
            }
        }
        public Order GetSelectedOrder()
        {
            if (DatagridXAML.SelectedItem is null) { return null; }
            return (Order)DatagridXAML.Items.GetItemAt(DatagridXAML.SelectedIndex);
            
        }

        private async void DeleteOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DatagridXAML.SelectedItem != null)
                {
                    OrderViewItem SelectedOrder = (OrderViewItem)DatagridXAML.Items.GetItemAt(DatagridXAML.SelectedIndex);
                    if (await vm.DeleteOrder(SelectedOrder.Id))
                    {
                        DatagridXAML.Items.Clear();
                        (await vm.GetOrdersAsync()).ForEach(order => DatagridXAML.Items.Add(order));
                    } else
                    {
                        throw new Exception("Deletion failed.");
                    }
                }
            } catch (Exception ex)
            {
                Log.Log($"Delete Order failed due to {ex}");
            }
        }

        private void SearchOrder(object sender, RoutedEventArgs e)
        {
            DatagridXAML.Items.Clear();
            try
            {
                vm.orderviewItems.ForEach(ovi => {
                    if (ovi.OrderName.Contains(searchOrderTextBox.Text)) { DatagridXAML.Items.Add(ovi); }
                    else if (ovi.ArticleNames.Contains(searchOrderTextBox.Text)) { DatagridXAML.Items.Add(ovi); }
                });

            }
            catch (WarningException ex)
            {
                Log.Log($"Search Orders went wrong due to: {ex.Message}");
            }
        }

        private void NewOrder(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new EditOrderArticlesViewModel();
        }

        private void EditOrder(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new EditOrderArticlesViewModel();
        }
    }
}

using System;
using System.Collections.Generic;
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

                (await vm.GetOrdersAsync()).ForEach(order => DatagridXAML.Items.Add(order));
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
                    Order SelectedOrder = (Order)DatagridXAML.Items.GetItemAt(DatagridXAML.SelectedIndex);
                    if (await vm.DeleteOrder(SelectedOrder))
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

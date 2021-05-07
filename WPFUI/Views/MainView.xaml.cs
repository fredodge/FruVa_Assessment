using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        Logger Log;
        MainViewModel vm;
        public MainView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                Log = new Logger();
                vm = new MainViewModel();
                vm.Load();

                foreach (var Order in vm.Orders)
                {
                    DatagridXAML.Items.Add(Order);
                }
            }
        }

        public Orders GetSelectedOrder()
        {
            Orders Order = (Orders)DatagridXAML.Items.GetItemAt(DatagridXAML.SelectedIndex);
            Log.Log($"Order: {Order.OrderName} {Order.Id}");
            return Order;
        }

        private void EditOrder(object sender, RoutedEventArgs e)
        {
            DataContext = new CreateOrderViewModel();
        }

        private void DeleteOrder(object sender, RoutedEventArgs e)
        {
            if (DatagridXAML.SelectedItem != null)
            {
                Orders SelectedOrder = (Orders)DatagridXAML.Items.GetItemAt(DatagridXAML.SelectedIndex);
                vm.DeleteOrder(SelectedOrder);
                DatagridXAML.Items.Clear();
                foreach (var Order in vm.Orders)
                {
                    DatagridXAML.Items.Add(Order);
                }
            }
        }
    }
}

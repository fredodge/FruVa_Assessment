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
using System.Windows.Forms;
using WPFUI.ViewModels;
using WPFUI.Views;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new OrderViewModel();
        }

        private void CreateOrder(object sender, RoutedEventArgs e)
        {
            DataContext = new CreateOrderViewModel();
        }
        public void EditOrder(object sender, RoutedEventArgs e)
        {
            DataContext = new EditOrderViewModel();
        }
        public void MainView(object sender, RoutedEventArgs e)
        {
            DataContext = new OrderViewModel();
        }
    }
}

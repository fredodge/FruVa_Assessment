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
        Logger Log;
        
        public MainWindow()
        {
            Log = new Logger();

            InitializeComponent();
            Log.Log($"MainWindow initialised.");
        }

        private void CreateOrder(object sender, RoutedEventArgs e)
        {
            DataContext = new CreateOrderViewModel();
        }
        private void EditOrder(object sender, RoutedEventArgs e)
        {
            DataContext = new EditOrderViewModel();
        }
        private void MainView(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }

    }
}

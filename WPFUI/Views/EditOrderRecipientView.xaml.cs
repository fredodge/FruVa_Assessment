using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPFUI.Models;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    /// <summary>
    /// Interaction logic for EditOrderRecipientView.xaml
    /// </summary>
    public partial class EditOrderRecipientView : UserControl
    {
        EditOrderRecipientViewModel vm;
        Logger Log;

        public EditOrderRecipientView()
        {
            InitializeComponent();
            UserControl_Loaded(null, null);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Log = new Logger();
                vm = (EditOrderRecipientViewModel) Application.Current.MainWindow.DataContext;
                vm.Load();

                if (!vm.order.RecipientId.Equals(Guid.Empty))
                {
                    labelRecipientName.Content = (await vm.GetRecipientByIdAsync(vm.order.RecipientId)).Name;
                }
                (await vm.GetRecipientsAsync()).ForEach(recipient => DatagridChooseRecipientsXAML.Items.Add(recipient));
            }
        }


        private async void SearchRecipients(object sender, RoutedEventArgs e)
        {
            DatagridChooseRecipientsXAML.Items.Clear();
            try
            {
                (await vm.GetRecipientsAsync()).ForEach(recipient => {
                    if (recipient.Name.Contains(searchRecipientsTextBox.Text))
                        DatagridChooseRecipientsXAML.Items.Add(recipient);
                });
            }
            catch (WarningException ex)
            {
                Log.Log($"Search Articles went wrong due to: {ex.Message}");
            }
        }

        private void ChangeRecipient(object sender, RoutedEventArgs e)
        {
            if (DatagridChooseRecipientsXAML.SelectedItem != null)
            {
                var recipient = (Recipient)DatagridChooseRecipientsXAML.Items.GetItemAt(DatagridChooseRecipientsXAML.SelectedIndex);
                vm.order.RecipientId = recipient.Id;
                labelRecipientName.Content = recipient.Name;
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new EditOrderArticlesViewModel(vm.order, vm.orderItems);
        }

        private void ContinueWithConclusion(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new SummaryViewModel(vm.order, vm.orderItems);
        }
    }
}

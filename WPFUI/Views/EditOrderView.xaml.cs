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
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    /// <summary>
    /// Interaction logic for EditOrderView.xaml
    /// </summary>
    public partial class EditOrderView : UserControl, INotifyPropertyChanged
    {
        EditOrderViewModel vm;
        MainViewModel mvm;
        Logger Log;
        Recipients CurrentRecipient;
        Orders OrderBeingEdited;

        private string _name;
        string CurrentRecipientName
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name == value) return;

                _name = value;
                OnPropertyChanged("CurrentRecipientName");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public EditOrderView()
        {
            Log = new Logger();
            InitializeComponent();
            CurrentRecipientName = "Choose Recipient";
            CurrentRecipient = new Recipients();
            OrderBeingEdited = new Orders();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                vm = new EditOrderViewModel();
                vm.Load();
                mvm = new MainViewModel();
                mvm.Load();

                foreach (var article in vm.articles_context)
                {
                    DatagridChooseArticleXAML.Items.Add(article);
                }

                foreach (var Order in mvm.Orders)
                {
                    DatagridChooseOrderXAML.Items.Add(Order);
                }
            }

            // 	//Load your data here and assign the result to the CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            if (DatagridChooseArticleXAML.SelectedItem != null)
            {
                DatagridCartXAML.Items.Add(DatagridChooseArticleXAML.SelectedItem);
                Log.Log($"Added: {((Articles)DatagridChooseArticleXAML.Items.GetItemAt(DatagridChooseArticleXAML.SelectedIndex)).ArticleName}");
            }

            if (DatagridChooseRecipientsXAML.SelectedItem != null)
            {
                CurrentRecipient = ((Recipients)DatagridChooseRecipientsXAML.Items.GetItemAt(DatagridChooseRecipientsXAML.SelectedIndex));
                CurrentRecipientName = CurrentRecipient.Name;
                Log.Log($"Recipients :: Set :: { CurrentRecipientName }");
            }
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            if (DatagridCartXAML.SelectedItem != null)
            {
                string removed = $"Removed: {((Articles)DatagridCartXAML.Items.GetItemAt(DatagridCartXAML.SelectedIndex)).ArticleName}";
                DatagridCartXAML.Items.Remove(DatagridCartXAML.SelectedItem);
                Log.Log(removed);
            }
        }

        private void EditOrder(object sender, RoutedEventArgs e)
        {
            List<Articles> Articles = new List<Articles>();
            foreach (var item in DatagridCartXAML.Items)
            {
                Articles.Add((Articles)item);
            }
            vm.EditOrder(Articles, OrderBeingEdited.Id);
            DatagridChooseOrderXAML.Items.Clear();
            DatagridCartXAML.Items.Clear();
            mvm.Load();
            foreach (var Order in mvm.Orders)
            {
                DatagridChooseOrderXAML.Items.Add(Order);
            }
        }

        private void StartEdit(object sender, RoutedEventArgs e)
        {
            if (DatagridChooseOrderXAML.SelectedItem != null)
            {
                if (OrderBeingEdited != null) { Log.Log("proof");  }
                var gridEntry = DatagridChooseOrderXAML.Items.GetItemAt(DatagridChooseOrderXAML.SelectedIndex);
                OrderBeingEdited = (Orders)gridEntry;
                List<OrderItems> OrdersOrderItems = new List<OrderItems>();
                foreach( var orderItem in mvm.OrderItems)
                {
                    if (orderItem.OrderId == OrderBeingEdited.Id)
                    {
                        OrdersOrderItems.Add(orderItem);
                    }
                }
                DatagridCartXAML.Items.Clear();
                foreach ( var item in OrdersOrderItems )
                {
                    DatagridCartXAML.Items.Add(vm.articles_context.Find(delegate (Articles article) { return article.Id == item.ArticleId; }));
                }
            }
        }
    }
}

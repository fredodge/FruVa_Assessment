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
    public partial class CreateOrderView : UserControl
    {
        CreateOrderViewModel vm;
        Logger Log;
        Recipients CurrentRecipient;
        string OrderName = "Order";

        public CreateOrderView()
        {
            Log = new Logger();
            InitializeComponent();
            CurrentRecipient = new Recipients();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                vm = new CreateOrderViewModel();
                vm.Load();

                foreach (var article in vm.articles_context)
                {
                    DatagridChooseArticleXAML.Items.Add(article);
                }

                foreach (var recipient in vm.recipients)
                {
                    DatagridChooseRecipientsXAML.Items.Add(recipient);
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
                Log.Log($"Added Article {((Articles) DatagridChooseArticleXAML.Items.GetItemAt(DatagridChooseArticleXAML.SelectedIndex)).Id} to Cart.");
            }

            if (DatagridChooseRecipientsXAML.SelectedItem != null)
            {
                CurrentRecipient = ((Recipients)DatagridChooseRecipientsXAML.Items.GetItemAt(DatagridChooseRecipientsXAML.SelectedIndex));
                Log.Log($"Made Recipient {((Articles)DatagridChooseArticleXAML.Items.GetItemAt(DatagridChooseArticleXAML.SelectedIndex)).Id} to Recipient of the Order.");
            }
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            if (DatagridCartXAML.SelectedItem != null)
            {
                string removed = $"Removed: {((Articles)DatagridCartXAML.Items.GetItemAt(DatagridCartXAML.SelectedIndex)).Id}";
                DatagridCartXAML.Items.Remove(DatagridCartXAML.SelectedItem);
                Log.Log($"Removed Article { removed } from Cart.");
            }
        }

        private void CreateOrder(object sender, RoutedEventArgs e)
        {
            try {
                List<Articles> Articles = new List<Articles>();
                foreach(var item in DatagridCartXAML.Items)
                {
                    Articles.Add((Articles)item);
                }
                vm.CreateOrder(Articles, CurrentRecipient, OrderName);
            } catch (Exception ex)
            {
                Log.Log($"Create Order went wrong due to: {ex.Message}");
            }
        }

        private void SearchArticle(object sender, RoutedEventArgs e)
        {
            DatagridChooseArticleXAML.Items.Clear();
            try
            {
                foreach (var article in vm.articles_context)
                {
                    if (article.SearchQuery.Contains(searchArticlesTextBox.Text))
                    {
                        DatagridChooseArticleXAML.Items.Add(article);
                    }
                }
            } catch(WarningException ex)
            {
                Log.Log($"Search Articles went wrong due to: {ex.Message}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Models;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    /// <summary>
    /// Interaction logic for EditOrderView.xaml
    /// </summary>
    public partial class EditOrderView : UserControl
    {
        EditOrderViewModel vm;
        OrderViewModel mvm;
        Logger Log;
        Recipient CurrentRecipient;
        Order OrderBeingEdited;

        public EditOrderView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                Log = new Logger();
                CurrentRecipient = new Recipient();
                OrderBeingEdited = new Order();
                vm = new EditOrderViewModel();
                mvm = new OrderViewModel();
                vm.Load();

                (await vm.GetArticlesAsync()).ForEach(article => DatagridChooseArticleXAML.Items.Add(article));
                (await vm.GetRecipientsAsync()).ForEach(recipient => DatagridChooseRecipientsXAML.Items.Add(recipient));
                (await vm.GetOrdersAsync()).ForEach(order => DatagridChooseOrderXAML.Items.Add(order));
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
                Log.Log($"Added article {((Article)DatagridChooseArticleXAML.Items.GetItemAt(DatagridChooseArticleXAML.SelectedIndex)).Id} to Cart.");
            }

            if (DatagridChooseRecipientsXAML.SelectedItem != null)
            {
                CurrentRecipient = ((Recipient)DatagridChooseRecipientsXAML.Items.GetItemAt(DatagridChooseRecipientsXAML.SelectedIndex));
                Log.Log($"Recipient { CurrentRecipient.Id } is now the Recipient of the Order.");
            }
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            if (DatagridCartXAML.SelectedItem != null)
            {
                string removed = $"{((Article)DatagridCartXAML.Items.GetItemAt(DatagridCartXAML.SelectedIndex)).Id}";
                DatagridCartXAML.Items.Remove(DatagridCartXAML.SelectedItem);
                Log.Log($"Removed article { removed } from Cart.");
            }
        }

        private void EditOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Article> Articles = new List<Article>();
                foreach (var item in DatagridCartXAML.Items)
                {
                    Articles.Add((Article)item);
                }
                vm.EditOrder(Articles, OrderBeingEdited.Id);
                
                DatagridChooseOrderXAML.Items.Clear();
                DatagridCartXAML.Items.Clear();
            } catch (Exception ex)
            {
                Log.Log($"Editing Order went wrong due to: {ex.Message}");
            }
        }

        private async void StartEdit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DatagridChooseOrderXAML.SelectedItem != null)
                {
                    OrderBeingEdited = (Order)DatagridChooseOrderXAML.Items.GetItemAt(DatagridChooseOrderXAML.SelectedIndex);

                    currentRecipientsTextBox.Text = CurrentRecipient.Name;

                    DatagridCartXAML.Items.Clear();

                    List<Article> orderedArticles = new List<Article>();
                    (await vm.GetOrderItemsByOrderAsync(OrderBeingEdited)).ForEach(async orderItem => {
                        orderedArticles.Add(await vm.GetArticleByIdAsync(orderItem.ArticleId));
                    });
                    Log.Log($"{orderedArticles.Count}");
                    orderedArticles.ForEach(article => DatagridCartXAML.Items.Add(article));
                }
            } catch (Exception ex)
            {
                Log.Log($"Order can not be edited due to: {ex.Message}");
            }
        }
    }
}

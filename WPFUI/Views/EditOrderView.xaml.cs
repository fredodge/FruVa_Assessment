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
        MainViewModel mvm;
        Logger Log;
        Recipient CurrentRecipient;
        Orders OrderBeingEdited;

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
                OrderBeingEdited = new Orders();
                vm = new EditOrderViewModel();
                mvm = new MainViewModel();
                vm.Load();
                mvm.Load();

                (await vm.GetArticlesAsync()).ForEach(article => DatagridChooseArticleXAML.Items.Add(article));
                (await vm.GetRecipientsAsync()).ForEach(recipient => DatagridChooseRecipientsXAML.Items.Add(recipient));

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
                Log.Log($"Added article {((Articles)DatagridChooseArticleXAML.Items.GetItemAt(DatagridChooseArticleXAML.SelectedIndex)).Id} to Cart.");
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
                string removed = $"{((Articles)DatagridCartXAML.Items.GetItemAt(DatagridCartXAML.SelectedIndex)).Id}";
                DatagridCartXAML.Items.Remove(DatagridCartXAML.SelectedItem);
                Log.Log($"Removed article { removed } from Cart.");
            }
        }

        private void EditOrder(object sender, RoutedEventArgs e)
        {
            try
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
            } catch (Exception ex)
            {
                Log.Log($"Editing Order went wrong due to: {ex.Message}");
            }
        }

        private void StartEdit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DatagridChooseOrderXAML.SelectedItem != null)
                {
                    OrderBeingEdited = (Orders)DatagridChooseOrderXAML.Items.GetItemAt(DatagridChooseOrderXAML.SelectedIndex);
                    List<OrderItems> OrdersOrderItems = new List<OrderItems>();

                    foreach (var orderItem in mvm.OrderItems)
                    {
                        if (orderItem.OrderId == OrderBeingEdited.Id)
                        {
                            OrdersOrderItems.Add(orderItem);
                        }
                    }/*
                    foreach (var recipient in (await vm.GetRecipientsAsync()))
                    {
                        if (OrderBeingEdited.RecipientId == recipient.Id)
                        {
                            CurrentRecipient = recipient;
                        }
                    }*/
                    currentRecipientsTextBox.Text = CurrentRecipient.Name;

                    DatagridCartXAML.Items.Clear();

                    /*
                    foreach (var item in OrdersOrderItems)
                    {
                        DatagridCartXAML.Items.Add(vm.articles_context.Find(delegate (Articles article) { return article.Id == item.ArticleId; }));
                    }*/
                }
            } catch (Exception ex)
            {
                Log.Log($"Order can not be edited due to: {ex.Message}");
            }
        }
    }
}

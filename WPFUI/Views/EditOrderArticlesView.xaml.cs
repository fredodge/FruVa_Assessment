﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Models;
using WPFUI.ViewModels;

namespace WPFUI.Views
{
    /// <summary>
    /// Interaction logic for EditOrderArticlesView.xaml
    /// </summary>
    public partial class EditOrderArticlesView : UserControl
    {
        EditOrderArticlesViewModel vm;
        Logger Log;

        public EditOrderArticlesView()
        {
            InitializeComponent();
            UserControl_Loaded(null, null);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Log = new Logger();
                vm = new EditOrderArticlesViewModel();
                vm.Load();
                for (int i = 1; i < 100; i++)
                {
                    addArticleAmount.Items.Add(i);
                    removeArticleAmount.Items.Add(i);
                }
                (await vm.GetArticlesAsync()).ForEach(article => DatagridChooseArticleXAML.Items.Add(article));
            }
        }

        private async void SearchArticles(object sender, RoutedEventArgs e)
        {
            DatagridChooseArticleXAML.Items.Clear();
            try
            {
                (await vm.GetArticlesAsync()).ForEach(article => {
                    if (article.SearchQuery.Contains(searchArticlesTextBox.Text)) { DatagridChooseArticleXAML.Items.Add(article); }
                });

            }
            catch (WarningException ex)
            {
                Log.Log($"Search Articles went wrong due to: {ex.Message}");
            }
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            if (DatagridChooseArticleXAML.SelectedItem != null)
            {
                var article = (Article)DatagridChooseArticleXAML.Items.GetItemAt(DatagridChooseArticleXAML.SelectedIndex);
                var orderItem = new OrderItem();
                orderItem.OrderId = vm.orderEditObject.Id;
                orderItem.ArticleId = article.Id;
                orderItem.Amount = (int) addArticleAmount.SelectedItem;
                vm.orderItemsEditList.Add(orderItem);

                DatagridCartXAML.Items.Add(CreateArticleOrderItem(article, orderItem));

                Log.Log($"Added {orderItem.Amount} times {((Article)DatagridChooseArticleXAML.Items.GetItemAt(DatagridChooseArticleXAML.SelectedIndex)).Id} to Cart.");
                clearSelections();
            }
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            if (DatagridCartXAML.SelectedItem != null)
            {
                var articleOrderItem = ((ArticleOrderItem)DatagridCartXAML.Items.GetItemAt(DatagridCartXAML.SelectedIndex));
                var removeAmount = (int)removeArticleAmount.SelectedItem;

                DatagridCartXAML.Items.Remove(DatagridCartXAML.SelectedItem);
                if (articleOrderItem.Amount - removeAmount < 1)
                {
                    vm.orderItemsEditList.Remove(((ArticleOrderItem)DatagridCartXAML.Items.GetItemAt(DatagridCartXAML.SelectedIndex)).OrderItem);
                } else
                {
                    articleOrderItem.Amount -= removeAmount;
                    articleOrderItem.OrderItem.Amount -= removeAmount;
                    DatagridCartXAML.Items.Add(articleOrderItem);
                }

                clearSelections();
            }
        }

        private void Abort(object sender, RoutedEventArgs e)
        {
            vm = null;
            Application.Current.MainWindow.DataContext = new OrderViewModel();
        }

        private void ContinueWithRecipientControl(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new EditOrderRecipientViewModel(vm.orderEditObject, vm.orderItemsEditList);
        }

        private ArticleOrderItem CreateArticleOrderItem(Article article, OrderItem orderItem)
        {
            ArticleOrderItem articleOrderItem = new ArticleOrderItem();

            articleOrderItem.Article = article;
            articleOrderItem.OrderItem = orderItem;

            articleOrderItem.Amount = orderItem.Amount;
            articleOrderItem.ArticleGroupName = article.ArticleGroupName;
            articleOrderItem.ArticleGroupNumber = article.ArticleGroupNumber;
            articleOrderItem.ArticleId = article.Id;
            articleOrderItem.ArticleName = article.ArticleName;
            articleOrderItem.Caliber = article.Caliber;
            articleOrderItem.Colli = article.Colli;
            articleOrderItem.DetailArticleNumber = article.DetailArticleNumber;
            articleOrderItem.Id = orderItem.Id;
            articleOrderItem.MainArticleNumber = article.MainArticleNumber;
            articleOrderItem.OrderId = orderItem.OrderId;
            articleOrderItem.OriginCountry = article.OriginCountry;
            articleOrderItem.OwnBrand = article.OwnBrand;
            articleOrderItem.PackageSize = article.PackageSize;
            articleOrderItem.RowVersion = article.RowVersion;
            articleOrderItem.SearchQuery = article.SearchQuery;
            articleOrderItem.TradeClass = article.TradeClass;
            articleOrderItem.Variety = article.Variety;

            return articleOrderItem;
        }

        private void clearSelections()
        {
            DatagridChooseArticleXAML.SelectedItems.Clear();
            DatagridCartXAML.SelectedItems.Clear();
            addArticleAmount.SelectedIndex = -1;
            removeArticleAmount.SelectedIndex = -1;
            searchArticlesTextBox.Clear();
        }
    }
}

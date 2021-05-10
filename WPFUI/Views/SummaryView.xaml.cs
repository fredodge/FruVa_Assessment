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
    /// Interaction logic for SummaryView.xaml
    /// </summary>
    public partial class SummaryView : UserControl
    {
        SummaryViewModel vm;
        Logger Log;

        public SummaryView()
        {
            InitializeComponent();
            UserControl_Loaded(null, null);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Log = new Logger();
                vm = (SummaryViewModel)Application.Current.MainWindow.DataContext;
                vm.Load();

                try
                {
                    List<ArticleOrderItem> articleOrderList = new List<ArticleOrderItem>();
                    vm.orderItems.ForEach(async orderItem => articleOrderList.Add(CreateArticleOrderItem(await vm.GetArticleByIdAsync(orderItem.ArticleId), orderItem)));
                    articleOrderList.ForEach(ao => listBoxArticles.Items.Add($"{ao.Amount} of {ao.ArticleName} in size {ao.PackageSize} coming from {ao.OriginCountry}"));

                    labelRecipientName.Content = (await vm.GetRecipientByIdAsync(vm.order.RecipientId)).Name;
                    int articleAmount = 0;
                    vm.orderItems.ForEach(oi => articleAmount += oi.Amount);
                    labelArticleCount.Content = articleAmount;
                } catch (Exception ep)
                {
                    Log.Log($"Something went wrong while loading data due to: {ep.Message}");
                }
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DataContext = new EditOrderRecipientViewModel(vm.order, vm.orderItems);
        }

        private async void Finish(object sender, RoutedEventArgs e)
        {
            Order order = new Order();
            try
            {
                order = await vm.ordersService.GetOrderByIdAsync(vm.order.Id);
            }
            catch (Exception ep) {
                Log.Log($"A new Order will be created ({ep.Message})");
            }

            try
            {
                if (!order.Id.Equals(Guid.Empty))
                {
                    (await vm.orderItemsService.GetOrderItemsByOrderAsync(order.Id)).ForEach(async oi => await vm.orderItemsService.DeleteOrderItem(oi.Id));
                    vm.order.DeliveryDay = datepickerDeliveryDay.SelectedDate.HasValue ? BitConverter.GetBytes(datepickerDeliveryDay.SelectedDate.Value.ToBinary()) : BitConverter.GetBytes(DateTime.Now.ToBinary());
                    vm.order.OrderName = "Name";

                    order = await vm.ordersService.PutOrder(vm.order);
                    vm.orderItems.ForEach(async oi =>
                    {
                        oi.OrderId = order.Id;
                        oi.Id = Guid.NewGuid();
                        await vm.orderItemsService.PostOrderItem(oi);
                    });
                }
                else
                {
                    vm.order.Id = Guid.NewGuid();
                    vm.order.OrderName = "Name";
                    vm.order.DeliveryDay = datepickerDeliveryDay.SelectedDate.HasValue ? BitConverter.GetBytes(datepickerDeliveryDay.SelectedDate.Value.ToBinary()) : BitConverter.GetBytes(DateTime.Now.ToBinary());

                    order = await vm.ordersService.PostOrder(vm.order);
                    vm.orderItems.ForEach(async oi =>
                    {
                        oi.OrderId = order.Id;
                        oi.Id = Guid.NewGuid();
                        await vm.orderItemsService.PostOrderItem(oi);
                    });
                }
            }
            catch (Exception ep)
            {
                Log.Log($"Editing/Creating Order went wrong due to: ({ep.Message})");
            }

            Application.Current.MainWindow.DataContext = new OrderViewModel();
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
    }
}

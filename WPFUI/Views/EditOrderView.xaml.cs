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
    /// Interaction logic for EditOrderView.xaml
    /// </summary>
    public partial class EditOrderView : UserControl
    {
        EditOrderViewModel eovm;
        public EditOrderView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            eovm = new EditOrderViewModel();
            eovm.Load();

            foreach (var articleGroup in eovm.articles_context)
            {
                foreach (var article in articleGroup.Value)
                {
                    DatagridChooseArticleXAML.Items.Add(article);
                }
            }

            foreach (var recipient in eovm.recipients)
            {
                DatagridChooseRecipientsXAML.Items.Add(recipient);
            }

            // Do not load your data at design time.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Load your data here and assign the result to the CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }
    }
}

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
using demo_ekz.Model;

namespace demo_ekz.Pages
{
    /// <summary>
    /// Interaction logic for HistorySalePage.xaml
    /// </summary>
    public partial class HistorySalePage : Page
    {
        public HistorySalePage(Product product)
        {
            InitializeComponent();
            LVEmployees.ItemsSource = product.ProductSale.ToList();
        }

        private void BackBtn_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SpisokPage());
        }
    }
}

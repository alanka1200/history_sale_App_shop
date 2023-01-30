using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
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
using System.Xml.Linq;
using demo_ekz.Model;


namespace demo_ekz.Pages
{
    /// <summary>
    /// Interaction logic for SpisokPage.xaml
    /// </summary>
    public partial class SpisokPage : Page
    {
        List<Product> products = new List<Product>();
        public SpisokPage()
        {
            InitializeComponent();
            var products = App.Db.Product.ToList();
            LVprod.ItemsSource = products;
            var allTypes = App.Db.Manufacturer.ToList();
            allTypes.Insert(0, new Manufacturer { Name = "Все" });
            DiscountSortCb.ItemsSource = allTypes;
            DiscountSortCb.SelectedIndex = 0;
            CBproiz.SelectedIndex = 0;
            Refresh();
        }

        public void Refresh()
        {
            var filterProduct = App.Db.Product.ToList();
            if (CBproiz.SelectedIndex == 1)
                filterProduct = filterProduct.OrderBy(x=> x.Cost).ToList();
            else if (CBproiz.SelectedIndex == 2)
                filterProduct = filterProduct.OrderByDescending(x => x.Cost).ToList(); //сортировка по убывания и возрастанию

            if (DiscountSortCb.SelectedIndex > 0)
            {
                var a = DiscountSortCb.SelectedIndex.ToString();
                filterProduct = filterProduct.Where(x => x.ManufacturerID.ToString().Contains(a.ToLower())).ToList(); //фильтрация по производителю
            }
            if (TitleDescriptionTb.Text.Length > 0)
            {
                filterProduct = filterProduct.Where(x => x.Title.ToLower().StartsWith(TitleDescriptionTb.Text.ToLower()) || x.Description.ToLower().StartsWith(TitleDescriptionTb.Text.ToLower())).ToList(); //шаблон рабоч поисковой строки
            }
            LVprod.ItemsSource = filterProduct.ToList(); //поисковая строка
            
        }
        private void TitleDescriptionTb_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }
        private void DiscountSortCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }
        private void CBproiz_SelectionChanged(object sender , SelectionChangedEventArgs e)
        {

            Refresh();
        }
        //   ПРОВЕРКА ПЕРЕД УДАЛЕНИЕМ, ПЕРЕЧЕТАТЬ ТЗ
        private void BTNdel_Click(object sender, RoutedEventArgs e) //удаление объекта
        {
            var product = LVprod.SelectedItem as Product;
            if (product != null)
            {
                if ( product.ProductSale.Count != 0 )
                {
                    MessageBox.Show("Продукт не может быть удален, поскольку у продукта есть история продаж",
                        "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (MessageBox.Show("Вы действительно хотите удалить эту запись?", "Уведомление",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    App.Db.Product.Remove(product);
                    App.Db.SaveChanges();
                    LVprod.ItemsSource = App.Db.Product.ToList();
                }
            }
        }

        private void BTNadd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPage());
        }
        private void BTNedt_Click(object sender, RoutedEventArgs e)
        {
            var select = LVprod.SelectedItem as Product;
            if (select == null)
            {
                MessageBox.Show("Выберите продукт");
                return;
            }
            NavigationService.Navigate(new AddEditPage(select));
        }


        private void BTNhist_OnClick(object sender, RoutedEventArgs e)
        {
            var selProduct = LVprod.SelectedItem as Product;
            if (selProduct == null)
            {
                MessageBox.Show("Выберите продукт");
                return;
            }
            NavigationService.Navigate(new HistorySalePage(selProduct));
        }
    }
}


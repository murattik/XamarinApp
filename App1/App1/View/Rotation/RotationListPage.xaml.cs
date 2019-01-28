using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.Rotation
{
    public class Item
    {
        public int ItemID { get; set; }
        public string Rfrom { get; set; }
        public int Rto { get; set; }
        public int CodeGoodsF { get; set; }
        public int CodeGoodsT { get; set; }
        public int PartF { get; set; }
        public int PartT { get; set; }
        public DateTime ValidateF { get; set; }
        public DateTime ValidateT { get; set; }
        public string PMF { get; set; }
        public string PMT { get; set; }
    }

    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Name { get; private set; }
        public Grouping(K name, IEnumerable<T> items)
        {
            Name = name;
            
            foreach (T item in items)
                Items.Add(item);
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RotationListPage : ContentPage
    {
        public ObservableCollection<Grouping<string, Item>> ItemGroups { get; set; }
        public List<Item> item { get; set; }

        public RotationListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
           item = new List<Item>
            {
                new Item { ItemID = 1, Rfrom = "1", Rto = 2, CodeGoodsF = 123, CodeGoodsT = 123, PartF = 111, PartT = 222,
                    ValidateF = DateTime.Today, ValidateT = DateTime.Today.AddDays(-1), PMF = "5/10", PMT = "5/10"  },

                new Item { ItemID = 2, Rfrom = "12", Rto = 20, CodeGoodsF = 1231, CodeGoodsT = 1231, PartF = 1114, PartT = 2224,
                    ValidateF = DateTime.Today, ValidateT = DateTime.Today.AddDays(-1), PMF = "1/10", PMT = "1/10" },

                new Item { ItemID = 3, Rfrom = "15", Rto = 22, CodeGoodsF = 1233, CodeGoodsT = 123, PartF = 1115, PartT = 2226,
                    ValidateF = DateTime.Today, ValidateT = DateTime.Today.AddDays(-5), PMF = "3/10", PMT = "5/10" },

                new Item { ItemID = 4, Rfrom = "17", Rto = 32, CodeGoodsF = 1243, CodeGoodsT = 123, PartF = 1113, PartT = 22233,
                    ValidateF = DateTime.Today, ValidateT = DateTime.Today.AddDays(-7), PMF = "5/10", PMT = "5/10" },

                new Item { ItemID = 5, Rfrom = "11", Rto = 21, CodeGoodsF = 1231, CodeGoodsT = 123, PartF = 11111, PartT = 22233,
                    ValidateF = DateTime.Today, ValidateT = DateTime.Today, PMF = "5/10", PMT = "1/10" },

                 new Item { ItemID = 6, Rfrom = "1", Rto = 21, CodeGoodsF = 123, CodeGoodsT = 123, PartF = 11111, PartT = 22233,
                    ValidateF = DateTime.Today, ValidateT = DateTime.Today, PMF = "5/10", PMT = "1/10" },
            };

            // получаем группы
            var groups = item.GroupBy(p => p.Rfrom).Select(g => new Grouping<string, Item>(g.Key, g));
            // передаем группы в PhoneGroups
            ItemGroups = new ObservableCollection<Grouping<string, Item>>(groups);

            base.OnAppearing();
            itemsList.ItemsSource = ItemGroups;
            CheckConnection(); //товары будут выведены только после проверки на подключение к сети
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            itemsList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                itemsList.ItemsSource = ItemGroups;
            else
                itemsList.ItemsSource = ItemGroups.Where(a => a.Name.Contains(e.NewTextValue)); 

            itemsList.EndRefresh();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            Item selectedItem = (Item)e.SelectedItem;
            RotationDetailPage RotationDetailPage = new RotationDetailPage();
            RotationDetailPage.BindingContext = selectedItem;
             Navigation.PushAsync(RotationDetailPage);
        }

        private async void CheckConnection()
        {
            //проверка на подключение к сети
            if (CrossConnectivity.Current != null &&
                CrossConnectivity.Current.ConnectionTypes != null &&
                CrossConnectivity.Current.IsConnected == true)
            {
                var connectionType = CrossConnectivity.Current.ConnectionTypes.FirstOrDefault();

                if (connectionType.ToString() == "Cellular")
                {
                    await DisplayAlert("Нет подключения к сети!", "Подключитесь к сети WiFi", "OK"); //если используется мобильный интернет - выводим ошибку
                }
                //else
                //{
                //    await viewModel.GetGoods(); // если WiFi, то загружаем данные с сервиса
                //}
            }
            else
            {
                //если подключение не обнаружено, то ошибка
                await DisplayAlert("Нет подключения к сети!", "Подключитесь к сети WiFi", "OK");
            }
        }
    }
}
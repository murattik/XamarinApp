using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.Rotation
{
    public class ItemColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Item item = value as Item;

                if (item.IsRotatoning == true)
                {
                    return Color.Silver;
                }
                else
                {
                    return Color.White;
                }
            }
            return Color.White; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class Item
    {
        public int ItemID { get; set; }
        public int Rfrom { get; set; } //ряд отправитель    
        public int Rto { get; set; } //ряд получатель
        public int CodeGoodsF { get; set; } //код товара
        public int CodeGoodsT { get; set; }//код товара (можно одним полем, т.к. он одинаковый должен быть для рядов)
        public int PartF { get; set; } //партия отправителя
        public int PartT { get; set; } //партия получателя
        public DateTime ValidateF { get; set; } //срок годности отправителя
        public DateTime ValidateT { get; set; } //срок годности получателя
        public string PMF { get; set; } //П-Мест отправителя
        public string PMT { get; set; } //П-Мест получателя
        public bool IsRotatoning { get; set; } //признак идёт ли ротация
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
        public ObservableCollection<Grouping<int, Item>> ItemGroups { get; set; }
        public List<Item> Item { get; set; }

        public RotationListPage()
        {
            InitializeComponent();
            RangeSlider.LowerValueChanged += RangeSliderOnLowerValueChanged;
            RangeSlider.UpperValueChanged += RangeSliderOnUpperValueChanged;
            PageRight.TranslationX = 1000;
            CrossConnectivity.Current.ConnectivityChanged += CurrentConnectivityChanged;


            Item = new List<Item>
                    {
                        new Item { ItemID = 1, Rfrom = 1, Rto = 2, CodeGoodsF = 123, CodeGoodsT = 123, PartF = 111, PartT = 222,
                            ValidateF = DateTime.Today, ValidateT = DateTime.Today.AddDays(-1), PMF = "5/10", PMT = "5/10", IsRotatoning = true },

                        new Item { ItemID = 2, Rfrom = 12, Rto = 20, CodeGoodsF = 1231, CodeGoodsT = 1231, PartF = 1114, PartT = 2224,
                            ValidateF = DateTime.Today, ValidateT = DateTime.Today.AddDays(-1), PMF = "1/10", PMT = "1/10", IsRotatoning = false},

                        new Item { ItemID = 3, Rfrom = 15, Rto = 22, CodeGoodsF = 1233, CodeGoodsT = 123, PartF = 1115, PartT = 2226,
                            ValidateF = DateTime.Today, ValidateT = DateTime.Today.AddDays(-5), PMF = "3/10", PMT = "5/10", IsRotatoning = true },

                        new Item { ItemID = 4, Rfrom = 17, Rto = 32, CodeGoodsF = 1243, CodeGoodsT = 123, PartF = 1113, PartT = 22233,
                            ValidateF = DateTime.Today, ValidateT = DateTime.Today.AddDays(-7), PMF = "5/10", PMT = "5/10", IsRotatoning = false },

                        new Item { ItemID = 5, Rfrom = 11, Rto = 21, CodeGoodsF = 1231, CodeGoodsT = 123, PartF = 11111, PartT = 22233,
                            ValidateF = DateTime.Today, ValidateT = DateTime.Today, PMF = "5/10", PMT = "1/10", IsRotatoning = false },

                            new Item { ItemID = 6, Rfrom = 1, Rto = 21, CodeGoodsF = 123, CodeGoodsT = 123, PartF = 11111, PartT = 22233,
                            ValidateF = DateTime.Today, ValidateT = DateTime.Today, PMF = "5/10", PMT = "1/10", IsRotatoning = false },
                    };

            string srchBar = searchBar.Text;//получаем текст из поиска

            if (srchBar != null && srchBar != "")
            {
                // получаем группы
                IEnumerable<Grouping<int, Item>> groups = Item.Where(a => a.Rfrom <= RangeSlider.UpperValue && a.Rfrom >= RangeSlider.LowerValue && a.CodeGoodsF.ToString().Contains(srchBar))
                                .OrderBy(a => a.Rfrom)
                                .GroupBy(p => p.Rfrom)
                                .Select(g => new Grouping<int, Item>(g.Key, g));
                // передаем группы в PhoneGroups
                ItemGroups = new ObservableCollection<Grouping<int, Item>>(groups);
            }
            else
            {
                // получаем группы
                IEnumerable<Grouping<int, Item>> groups = Item.Where(a => a.Rfrom <= RangeSlider.UpperValue && a.Rfrom >= RangeSlider.LowerValue)
                                .OrderBy(a => a.Rfrom)
                                .GroupBy(p => p.Rfrom)
                                .Select(g => new Grouping<int, Item>(g.Key, g));
                // передаем группы в PhoneGroups
                ItemGroups = new ObservableCollection<Grouping<int, Item>>(groups);
            }


            itemsList.ItemsSource = ItemGroups;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CheckConnection();
        }

        //на страницу с фольтром
        async void ToFilter_Tapped(object sender, System.EventArgs e)
        {
            await PageRight.TranslateTo(0, 0, 500, Easing.SinIn);
        }

        //на основную страницу
        async void RightFilter_Tapped(object sender, System.EventArgs e)
        {
            await PageRight.TranslateTo(Page.Width, 0, 500, Easing.SinIn);
        }

        //private void PageRefresh(object sender, EventArgs eventArgs)
        //{
        //    Navigation.PopAsync();
        //    Navigation.PushAsync(new RotationListPage());

        //}

        private void Switcher_Toggled(object sender, ToggledEventArgs e)
        {
            string srchBar = searchBar.Text;//получаем текст из поиска

            if (e.Value == true)
            {
                label.Text = String.Format("Ротируемые");
                itemsList.BeginRefresh(); //обязательное действие

                if (srchBar != null && srchBar != "")
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom >= RangeSlider.LowerValue && a.Rfrom <= RangeSlider.UpperValue && a.CodeGoodsF.ToString().Contains(srchBar) && a.IsRotatoning == true)
                                                                               .OrderBy(a => a.Rfrom)
                                                                               .GroupBy(p => p.Rfrom)
                                                                               .Select(g => new Grouping<int, Item>(g.Key, g)));
                }
                else
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom >= RangeSlider.LowerValue && a.Rfrom <= RangeSlider.UpperValue && a.IsRotatoning == true)
                                                                               .OrderBy(a => a.Rfrom)
                                                                               .GroupBy(p => p.Rfrom)
                                                                               .Select(g => new Grouping<int, Item>(g.Key, g)));
                }

                itemsList.ItemsSource = ItemGroups;
                itemsList.EndRefresh(); //обязательное действие
            }
            else
            {
                label.Text = String.Format("Все");
                itemsList.BeginRefresh(); //обязательное действие

                if (srchBar != null && srchBar != "")
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom >= RangeSlider.LowerValue && a.Rfrom <= RangeSlider.UpperValue && a.CodeGoodsF.ToString().Contains(srchBar))
                                                                             .OrderBy(a => a.Rfrom)
                                                                             .GroupBy(p => p.Rfrom)
                                                                             .Select(g => new Grouping<int, Item>(g.Key, g)));
                }
                else
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom >= RangeSlider.LowerValue && a.Rfrom <= RangeSlider.UpperValue)
                                                                             .OrderBy(a => a.Rfrom)
                                                                             .GroupBy(p => p.Rfrom)
                                                                             .Select(g => new Grouping<int, Item>(g.Key, g)));
                }


                itemsList.ItemsSource = ItemGroups;
                itemsList.EndRefresh(); //обязательное действие
            }

       
        }

        private void RangeSliderOnUpperValueChanged(object sender, EventArgs eventArgs) //фильтр по максимальному значению
        {
            itemsList.BeginRefresh(); //обязательное действие

            string srchBar = searchBar.Text;//получаем текст из поиска

            if (srchBar != null && srchBar != "")
            {
                if (tglRotation.IsToggled == true)
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom <= RangeSlider.UpperValue && a.Rfrom >= RangeSlider.LowerValue 
                                                                                && a.CodeGoodsF.ToString().Contains(srchBar) && a.IsRotatoning == true)
                                                .OrderBy(a => a.Rfrom)
                                                .GroupBy(p => p.Rfrom)
                                                .Select(g => new Grouping<int, Item>(g.Key, g))); 
                }
                else
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom <= RangeSlider.UpperValue && a.Rfrom >= RangeSlider.LowerValue && a.CodeGoodsF.ToString().Contains(srchBar))
                                                .OrderBy(a => a.Rfrom)
                                                .GroupBy(p => p.Rfrom)
                                                .Select(g => new Grouping<int, Item>(g.Key, g))); 
                }

                
                itemsList.ItemsSource = ItemGroups;
            }

            else
            {
                if (tglRotation.IsToggled == true)
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom <= RangeSlider.UpperValue && a.Rfrom >= RangeSlider.LowerValue && a.IsRotatoning == true)
                                                                .OrderBy(a => a.Rfrom)
                                                                .GroupBy(p => p.Rfrom)
                                                                .Select(g => new Grouping<int, Item>(g.Key, g))); 
                }
                else
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom <= RangeSlider.UpperValue && a.Rfrom >= RangeSlider.LowerValue)
                                                                .OrderBy(a => a.Rfrom)
                                                                .GroupBy(p => p.Rfrom)
                                                                .Select(g => new Grouping<int, Item>(g.Key, g))); 
                }
                
                itemsList.ItemsSource = ItemGroups;
            }
            
            itemsList.EndRefresh(); //обязательное действие
        }

        private void RangeSliderOnLowerValueChanged(object sender, EventArgs eventArgs) //фильтр по минимальному значению
        {
            itemsList.BeginRefresh(); //обязательное действие

            string srchBar = searchBar.Text;//получаем текст из поиска

            if (srchBar != null && srchBar != "")
            {
                if (tglRotation.IsToggled == true)
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom >= RangeSlider.LowerValue && a.Rfrom <= RangeSlider.UpperValue 
                                                                                    && a.CodeGoodsF.ToString().Contains(srchBar) && a.IsRotatoning == true)
                                                                           .OrderBy(a => a.Rfrom)
                                                                           .GroupBy(p => p.Rfrom)
                                                                           .Select(g => new Grouping<int, Item>(g.Key, g))); //поиск по коду товара
                }
                else
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom >= RangeSlider.LowerValue && a.Rfrom <= RangeSlider.UpperValue && a.CodeGoodsF.ToString().Contains(srchBar))
                                                                            .OrderBy(a => a.Rfrom)
                                                                            .GroupBy(p => p.Rfrom)
                                                                            .Select(g => new Grouping<int, Item>(g.Key, g))); //поиск по коду товара
                }

                itemsList.ItemsSource = ItemGroups;
            }
            else
            {
                if (tglRotation.IsToggled == true)
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom >= RangeSlider.LowerValue && a.Rfrom <= RangeSlider.UpperValue && a.IsRotatoning == true)
                                                                .OrderBy(a => a.Rfrom)
                                                                .GroupBy(p => p.Rfrom)
                                                                .Select(g => new Grouping<int, Item>(g.Key, g))); //поиск по коду товара
                }
                else
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom >= RangeSlider.LowerValue && a.Rfrom <= RangeSlider.UpperValue)
                                                                .OrderBy(a => a.Rfrom)
                                                                .GroupBy(p => p.Rfrom)
                                                                .Select(g => new Grouping<int, Item>(g.Key, g))); //поиск по коду товара
                }
                
                itemsList.ItemsSource = ItemGroups;
            }

            itemsList.EndRefresh(); //обязательное действие

        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e) //поиск
        {
            itemsList.BeginRefresh(); //обязательное действие

            if (string.IsNullOrWhiteSpace(e.NewTextValue) || e.NewTextValue == "")
            {
                if (tglRotation.IsToggled != true)
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom >= RangeSlider.LowerValue && a.Rfrom <= RangeSlider.UpperValue)
                                                                              .OrderBy(a => a.Rfrom)
                                                                              .GroupBy(p => p.Rfrom)
                                                                              .Select(g => new Grouping<int, Item>(g.Key, g)));
                }
                else
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.Rfrom >= RangeSlider.LowerValue && a.Rfrom <= RangeSlider.UpperValue && a.IsRotatoning == true)
                                                                              .OrderBy(a => a.Rfrom)
                                                                              .GroupBy(p => p.Rfrom)
                                                                              .Select(g => new Grouping<int, Item>(g.Key, g)));
                }

            }
            else
            {
                if (tglRotation.IsToggled == true)
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.CodeGoodsF.ToString().Contains(e.NewTextValue)
                                                                                && a.Rfrom >= RangeSlider.LowerValue && a.Rfrom <= RangeSlider.UpperValue && a.IsRotatoning == true)
                                                                                .OrderBy(a => a.Rfrom)
                                                                                .GroupBy(p => p.Rfrom)
                                                                                .Select(g => new Grouping<int, Item>(g.Key, g)));
                }
                else
                {
                    ItemGroups = new ObservableCollection<Grouping<int, Item>>(Item.Where(a => a.CodeGoodsF.ToString().Contains(e.NewTextValue)
                                                                                 && a.Rfrom >= RangeSlider.LowerValue && a.Rfrom <= RangeSlider.UpperValue)
                                                                                 .OrderBy(a => a.Rfrom)
                                                                                 .GroupBy(p => p.Rfrom)
                                                                                 .Select(g => new Grouping<int, Item>(g.Key, g)));
                }
                
                
            }

            itemsList.ItemsSource = ItemGroups;
            itemsList.EndRefresh(); //обязательное действие
        }

       

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            Item selectedItem = (Item)e.SelectedItem;
            RotationDetailPage RotationDetailPage = new RotationDetailPage
            {
                BindingContext = selectedItem
            };
            Navigation.PushAsync(RotationDetailPage);
        }

        // обработка изменения состояния подключения
        private void CurrentConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            CheckConnection();
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
                    await Application.Current.MainPage.DisplayAlert("Нет подключения к сети!", "Подключитесь к сети WiFi", "OK");
                    //await DisplayAlert("Нет подключения к сети!", "Подключитесь к сети WiFi", "OK"); //если используется мобильный интернет - выводим ошибку
                }
                else
                {
                    //await viewModel.GetGoods(); // если WiFi, то загружаем данные с сервиса
                    
                }
            }
            else
            {
                //если подключение не обнаружено, то ошибка
                await Application.Current.MainPage.DisplayAlert("Нет подключения к сети!", "Подключитесь к сети WiFi", "OK");
            }
        }

    }
}
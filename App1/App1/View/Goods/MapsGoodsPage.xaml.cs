using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapsGoodsPage : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public MapsGoodsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            MapsGoodsList.ItemsSource = App.MapsDatabase.GetItems();
            base.OnAppearing();
        }


        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MapsGoods selectedGoods = (MapsGoods)e.SelectedItem;
            MapsGoodsDetails detailGoodsPage = new MapsGoodsDetails();
            detailGoodsPage.BindingContext = selectedGoods;
            await Navigation.PushAsync(detailGoodsPage);
        }
        // обработка нажатия кнопки добавления
        private async void CreateBarecode(object sender, EventArgs e)
        {
            MapsGoods result = new MapsGoods();
            MapsGoodsDetails barecodePage = new MapsGoodsDetails();

            //result.Status = 1;
            //result.dateTime = DateTime.Now;

            barecodePage.BindingContext = result;
            await Navigation.PushAsync(barecodePage);
        }
    }
}

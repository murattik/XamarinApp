using App1.ViewModel;
using Plugin.Connectivity;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GoodsListPage : ContentPage
	{
        ApplicationViewModel viewModel;

        public GoodsListPage ()
		{
			InitializeComponent ();
            viewModel = new ApplicationViewModel() { Navigation = this.Navigation };
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CheckConnection(); //товары будут выведены только после проверки на подключение к сети
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
                else
                {
                    await viewModel.GetGoods(); // если WiFi, то загружаем данные с сервиса
                }
            }
            else
            {
                //если подключение не обнаружено, то ошибка
                await DisplayAlert("Нет подключения к сети!", "Подключитесь к сети WiFi", "OK");
            }
        }
    }
}
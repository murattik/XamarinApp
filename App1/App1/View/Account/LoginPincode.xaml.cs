using App1.ViewModel;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.Account
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPincode : ContentPage
	{
        UsersViewModel viewModel;

        public LoginPincode ()
		{
			InitializeComponent ();
            viewModel = new UsersViewModel() { Navigation = this.Navigation };
            BindingContext = viewModel;
            pin.Text = string.Empty;
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    CheckConnection(); //товары будут выведены только после проверки на подключение к сети
        //}

        //private async void CheckConnection()
        //{
        //    //проверка на подключение к сети
        //    if (CrossConnectivity.Current != null &&
        //        CrossConnectivity.Current.ConnectionTypes != null &&
        //        CrossConnectivity.Current.IsConnected == true)
        //    {
        //        var connectionType = CrossConnectivity.Current.ConnectionTypes.FirstOrDefault();

        //        if (connectionType.ToString() == "Cellular")
        //        {
        //            await DisplayAlert("Нет подключения к сети!", "Подключитесь к сети WiFi", "OK"); //если используется мобильный интернет - выводим ошибку
        //        }
        //        else
        //        {
        //            //await viewModel.GetUsers(); // если WiFi, то загружаем данные с сервиса
        //        }
        //    }
        //    else
        //    {
        //        //если подключение не обнаружено, то ошибка
        //        await DisplayAlert("Нет подключения к сети!", "Подключитесь к сети WiFi", "OK");
        //    }
        //}
    }
}
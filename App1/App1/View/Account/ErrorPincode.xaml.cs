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
	public partial class ErrorPincode : ContentPage
	{
		public ErrorPincode ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            UsersList.ItemsSource = App.UsersDatabase.GetItems();
            base.OnAppearing();
        }

        private async void Login(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void LoginPincode(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new LoginPincode());

            //удаление текущей страницы из стека - для возврата к предыдущей
            await this.Navigation.PopAsync();
        }
    }
}
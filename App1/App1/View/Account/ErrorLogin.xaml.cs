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
	public partial class ErrorLogin : ContentPage
	{
		public ErrorLogin ()
		{
			InitializeComponent ();
		}

        private async void Login(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
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
	public partial class UserOrgAndSkladPage : ContentPage
	{
		public UserOrgAndSkladPage ()
		{
			InitializeComponent();
		}

        protected override void OnAppearing()
        {
            UsersOrgAndSkladList.ItemsSource = App.UsersOrgAndSkladDatabase.GetItems();
            base.OnAppearing();
        }
    }
}
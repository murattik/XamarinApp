using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.Rotation
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RotationDetailPage : ContentPage
	{
		public RotationDetailPage ()
		{
			InitializeComponent ();
		}
        private void OK(object sender, EventArgs e)
        {
             DisplayAlert("Перемещение", "Перемещено!", "OK");
             this.Navigation.PopAsync();
        }
        private void Cancel(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }
}
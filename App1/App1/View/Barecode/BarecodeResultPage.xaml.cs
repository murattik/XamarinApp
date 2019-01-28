using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BarecodeResultPage : ContentPage
	{
		public BarecodeResultPage()
		{
			InitializeComponent ();
		}

        private void SaveBarecode(object sender, EventArgs e)
        {
            var barecode = (BarecodeResult)BindingContext;
            if (!String.IsNullOrEmpty(barecode.BarecodeFormat))
            {
                barecode.Status = 1;
                App.Database.SaveItem(barecode);
            }
            this.Navigation.PopAsync();
        }

        private void DeleteBarecode(object sender, EventArgs e)
        {
            var barecode = (BarecodeResult)BindingContext;
            App.Database.DeleteItem(barecode.Id);
            this.Navigation.PopAsync();
        }
        private void Cancel(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }
}
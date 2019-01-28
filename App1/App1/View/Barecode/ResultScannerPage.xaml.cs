using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResultScannerPage : ContentPage
	{
		public ResultScannerPage()
		{
			InitializeComponent ();
          
        }

        protected override void OnAppearing()
        {
            barecodeList.ItemsSource = App.Database.GetItems();
            base.OnAppearing();
        }


        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            BarecodeResult selectedBarecode = (BarecodeResult)e.SelectedItem;
            BarecodeResultPage barecodePage = new BarecodeResultPage();
            barecodePage.BindingContext = selectedBarecode;
            await Navigation.PushAsync(barecodePage);
        }
        // обработка нажатия кнопки добавления
        private async void CreateBarecode(object sender, EventArgs e)
        {
            BarecodeResult result = new BarecodeResult();
            BarecodeResultPage barecodePage = new BarecodeResultPage();

            result.Status = 1;
            result.dateTime = DateTime.Now;

            barecodePage.BindingContext = result;
            await Navigation.PushAsync(barecodePage);
        }
    }
}
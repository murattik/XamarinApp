using App1.ViewModel;
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
	public partial class GoodsPage : ContentPage
	{
        public tdGoods Model { get; private set; }
        public ApplicationViewModel ViewModel { get; private set; }

        public GoodsPage (tdGoods model, ApplicationViewModel viewModel)
		{
			InitializeComponent ();
            Model = model;
            ViewModel = viewModel;
            BindingContext = this;
        }
	}
}
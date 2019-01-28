using App1.Model;
using App1.View.Account;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.View.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPage : ContentPage
    {
        Label connectionStateLbl;
        Label connectionDetailsLbl;

        public AdminPage()
        {
            InitializeComponent();
            //отрисовываем кнопку в коде, дальнейшее рисование элементов на странице будет не достуно
            Title = "Страница администратора";

            connectionStateLbl = new Label
            {
                Text = "Подключение отсутствует",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            connectionDetailsLbl = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            CrossConnectivity.Current.ConnectivityChanged += CurrentConnectivityChanged;

            //кнопка открытия страницы сканера
            Button toAllUsersPageBtn = new Button
            {
                Text = "Список пользователей",
                //BackgroundColor = Color.Green,
                BackgroundColor = Color.FromHex("#12B812"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BorderWidth = 2,
                BorderColor = Color.White
            };

            //begin test btn
            Button toUserPage = new Button
            {
                Text = "Текущий пользователь",
                //BackgroundColor = Color.Red,
                BackgroundColor = Color.FromHex("#12B812"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BorderWidth = 2,
                BorderColor = Color.White
            };
            Button toUserOrgAndSkladPage = new Button
            {
                Text = "Список клиентов пользователя",
                //BackgroundColor = Color.Aquamarine,
                BackgroundColor = Color.FromHex("#12B812"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BorderWidth = 2,
                BorderColor = Color.White
                //WidthRequest = 177,
                //HeightRequest = 174
            };
            Button toGoodsList = new Button
            {
                Text = "Список товаров",
                //BackgroundColor = Color.YellowGreen,
                BackgroundColor = Color.FromHex("#12B812"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BorderWidth = 2,
                BorderColor = Color.White
            };
            Button changeOrgOrSklad = new Button
            {
                Text = "Сменить клиента/склад",
                //BackgroundColor = Color.SaddleBrown,
                BackgroundColor = Color.FromHex("#12B812"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BorderWidth = 2,
                BorderColor = Color.White
            };
            Button Login = new Button
            {
                Text = "Login",
                BackgroundColor = Color.FromHex("#12B812"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BorderWidth = 2,
                BorderColor = Color.White
            };
            //end test btn

            Grid grid = new Grid
            {
                RowDefinitions = //строки
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
            },
                ColumnDefinitions = //колонки
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            }
            };

            //позиции в строках и колонках
            grid.Children.Add(toAllUsersPageBtn, 0, 0);
            grid.Children.Add(toUserPage, 0, 1);
            grid.Children.Add(changeOrgOrSklad, 0, 2);
            grid.Children.Add(connectionStateLbl, 0, 3);
            grid.Children.Add(connectionDetailsLbl, 0, 3);
            Grid.SetColumnSpan(connectionStateLbl, 2);
            Grid.SetColumnSpan(connectionDetailsLbl, 2);

            grid.Children.Add(toUserOrgAndSkladPage, 1, 0);
            grid.Children.Add(toGoodsList, 1, 1);
            grid.Children.Add(Login, 1, 2);


            //отлавливаем нажатие кнопки сканирования
            toAllUsersPageBtn.Clicked += ToAllUsersPageBtn;

            //нажатие кнопки на страницу работы с БД
            toUserPage.Clicked += ToUserPage;

            //тестовые кнопки
            toUserOrgAndSkladPage.Clicked += ToUserOrgAndSkladPage;
            toGoodsList.Clicked += ToGoodsListPage;
            changeOrgOrSklad.Clicked += ToСhangeOrgOrSklad;
            Login.Clicked += ToLoginPage;

            //добавляем элементы на страницу 
            //Content = new StackLayout { Children = { toScannerPageBtn } };
            Content = grid;
        }


        private async void ToAllUsersPageBtn(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllUsersPage());
        }


        private async void ToUserPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserPage());
        }

        private async void ToUserOrgAndSkladPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserOrgAndSkladPage());
        }


        private async void ToLoginPage(object sender, EventArgs e)
        {
            //await DisplayAlert("Кнопка", "Скоро появится", "ОK");
            await Navigation.PushAsync(new LoginPage());
        }

        private async void ToСhangeOrgOrSklad(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrgForUserPage());
        }

        private async void ToGoodsListPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GoodsListPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CheckConnection();
        }

        // обработка изменения состояния подключения
        private void CurrentConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            CheckConnection();
        }
        // получаем состояние подключения
        private void CheckConnection()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentsPath, "barecode.db");

            //получаем место хранения БД
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(path);
            SQLiteConnection database = new SQLiteConnection(databasePath);

            string userName = database.Table<Users>().Select(a => a.MapsUserName).FirstOrDefault().ToString();
            if (userName == "" | userName == null)//если каким-то образом пользователь не определился или отсутствовал в бд, то устанавливается значение НЕОПРЕДЕЛЕНО
            {
                userName = "НЕОПРЕДЕЛЕНО";
            }

            connectionStateLbl.IsVisible = !CrossConnectivity.Current.IsConnected;
            connectionDetailsLbl.IsVisible = CrossConnectivity.Current.IsConnected;

            if (CrossConnectivity.Current != null &&
                CrossConnectivity.Current.ConnectionTypes != null &&
                CrossConnectivity.Current.IsConnected == true)
            {
                var connectionType = CrossConnectivity.Current.ConnectionTypes.FirstOrDefault();

                if (connectionType.ToString() == "Cellular")
                {
                    connectionDetailsLbl.Text = "Подключено к GSM! " + "\n" + "Подключитесь к WIFI!";
                }
                else
                {
                    connectionDetailsLbl.Text = "Пользователь: " + "\n" + userName;
                }
            }
        }
    }
}
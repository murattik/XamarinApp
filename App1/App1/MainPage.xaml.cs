using System;
using Xamarin.Forms;
using Plugin.Connectivity;
using System.Linq;
using App1.View.Account;
using SQLite;
using System.IO;
using App1.Model;
using App1.View.Admin;
using App1.View.Rotation;

namespace App1
{
    public partial class MainPage : ContentPage
    {

        Label connectionStateLbl;
        Label connectionDetailsLbl;

        public MainPage()
        {
            
            //отрисовываем кнопку в коде, дальнейшее рисование элементов на странице будет не достуно
            Title = "Домашняя страница";

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
            Button toAdminPageBtn = new Button
            {
                Text = "AdminPage",
                //BackgroundColor = Color.Green,
                BackgroundColor = Color.FromHex("#12B812"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BorderWidth = 2,
                BorderColor = Color.White
            };

            //begin test btn
            Button toDBpage = new Button
            {
                Text = "Результаты сканирования",
                //BackgroundColor = Color.Red,
                BackgroundColor = Color.FromHex("#12B812"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BorderWidth = 2,
                BorderColor = Color.White
            };
            Button toScanner = new Button
            {
                Text = "Сканер",
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
                Text = "Загрузить товары",
                //BackgroundColor = Color.YellowGreen,
                BackgroundColor = Color.FromHex("#12B812"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BorderWidth = 2,
                BorderColor = Color.White
            };
            Button changeOrgOrSklad = new Button
            {
                Text = "Смена клиента/склада",
                //BackgroundColor = Color.SaddleBrown,
                BackgroundColor = Color.FromHex("#12B812"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BorderWidth = 2,
                BorderColor = Color.White,
                IsVisible = false
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

            var user = App.UsersDatabase.GetItems();
            foreach (var data in user)
            {
                if (data.NameOrg != null && data.SkladName != null)
                {
                    changeOrgOrSklad.IsVisible = true;
                }
            }

            //позиции в строках и колонках
            grid.Children.Add(toAdminPageBtn, 0, 0);
            grid.Children.Add(toDBpage, 0, 1);
            grid.Children.Add(changeOrgOrSklad, 0, 2);
            grid.Children.Add(connectionStateLbl, 0, 3);
            grid.Children.Add(connectionDetailsLbl, 0, 3);
            Grid.SetColumnSpan(connectionStateLbl, 2);
            Grid.SetColumnSpan(connectionDetailsLbl, 2);

            grid.Children.Add(toScanner, 1, 0);
            grid.Children.Add(toGoodsList, 1, 1);
            grid.Children.Add(Login, 1, 2);


            //отлавливаем нажатие кнопки сканирования
            toAdminPageBtn.Clicked += ToAdminPage;

            //нажатие кнопки на страницу работы с БД
            toDBpage.Clicked += ToDBpageWork;

            //тестовые кнопки
            toScanner.Clicked += ToScannerPage;
            toGoodsList.Clicked += ToGoodsListPage;
            changeOrgOrSklad.Clicked += ToChangeOrgOrSklad;
            Login.Clicked += ToLoginPage;

            //добавляем элементы на страницу 
            //Content = new StackLayout { Children = { toScannerPageBtn } };
            Content = grid;

        }

        private async void ToLoginPage(object sender, EventArgs e)
        {
            //await DisplayAlert("Кнопка", "Скоро появится", "ОK");
            await Navigation.PushAsync(new LoginPage());
        }

        private async void ToChangeOrgOrSklad(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrgForUserPage());
        }

        private async void ToAdminPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminPage());
        }

        //действие нажатия кнопки работы с БД
        private async void ToDBpageWork(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new ResultScannerPage());
            await Navigation.PushAsync(new RotationListPage());
        }

        //Страница товаров
        private async void ToScannerPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScannerPage());
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

            string userName = null;
            string orgName = null;
            string skladName = null;
            var data = App.UsersDatabase.GetItems();

            foreach (var i in data)
            {
                userName = i.MapsUserName;
                orgName = i.NameOrg;
                skladName = i.SkladName;
            }
            
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
                    connectionDetailsLbl.Text = "Пользователь: " + "\n"  + userName + "\n" + "Клиент: " + orgName + "\n" + "Склад: " + skladName;
                }
            }
        }
    }

}

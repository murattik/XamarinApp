using App1.Model;
using Lamp.Plugin;
using SQLite;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScannerPage : ContentPage
	{
        public ScannerPage()
        {
            Title = "Сканер";


            //кнопка сканирования
            Button toScan = new Button
            {
                Text = "Сканировать",
                BackgroundColor = Color.Green,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                FontSize = 32,
                BorderWidth = 3,
                BorderColor = Color.White
            };

            //действие нажатия сканирования
            toScan.Clicked += OnClickScan;

            Grid grid = new Grid
            {
                RowDefinitions = { new RowDefinition { Height = new GridLength(1, GridUnitType.Star) } },
                ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) } }
            };
            grid.Children.Add(toScan);
            //добавляем созданные элементы на страницу
            Content = grid;
        }

        //событие клика сканирования
        public void OnClickScan(object sender, EventArgs e)
        {
            ScanAsync();
        }

        //сканирование штрихкода
        public void ScanAsync()
        {
            ZXingScannerView zxing = new ZXingScannerView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                IsAnalyzing = true,
                IsScanning = true,
            };


            //промежуток между сканированием
            zxing.Options.DelayBetweenContinuousScans = 3000;
            zxing.Options.BuildMultiFormatReader();

            //результат сканирования
            zxing.OnScanResult += (result) => 
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //получение места и имя БД
                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    var path = Path.Combine(documentsPath, "barecode.db");
                    string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(path);
                    SQLiteConnection database = new SQLiteConnection(databasePath);

                    //проверка есть ли такой штрихкод в локальной таблице товаров
                    var codeGoodsCount = database.Table<MapsGoods>().Where(a => a.BarCode == result.Text).Count(); //получаем количество таких штрихкодов
                    var codeGoods = database.Table<MapsGoods>().Where(a => a.BarCode == result.Text).Select(a => a.CodeGoods).FirstOrDefault(); //получаем код товара
                    var userName = database.Table<Users>().Select(a => a.MapsUserName).FirstOrDefault().ToString(); //получаем текущего пользователя

                    if(userName == "" | userName == null)//если каким-то образом пользователь не определился или отсутствовал в бд, то устанавливается значение НЕОПРЕДЕЛЕНО
                    {
                        userName = "НЕОПРЕДЕЛЕНО";
                    }

                    if (codeGoodsCount != 0 && codeGoods != null) //если данных штрихкод есть в локальной БД, то выводим код товара, если нет, то выводим "нет совпадений"
                    {
                        DisplayAlert("Код товара: " + codeGoods, "Штрихкод: " + result.Text, "OK");
                    }
                    else
                        DisplayAlert("Нет совпадений", "Штрихкод: " + result.Text, "OK");


                    //добавление в БД полученного результата сканирования
                    BarecodeResult barecodeResult = new BarecodeResult();
                    barecodeResult.BarecodeFormat = result.BarcodeFormat.ToString();
                    barecodeResult.TextResult = result.Text;
                    barecodeResult.CodeGoods = codeGoods;
                    barecodeResult.Status = 0;
                    barecodeResult.dateTime = DateTime.Now;
                    barecodeResult.UserName = userName;

                    if (!String.IsNullOrEmpty(barecodeResult.TextResult))
                    {
                        try
                        {
                            App.Database.SaveItem(barecodeResult);
                        }
                        catch
                        {
                            DisplayAlert("Ошибка", "В процессе добавления произошла ошибка", "OK");
                        }
                        
                    }
                    else
                    {
                        DisplayAlert("Ошибка", "Данные не добавлены.", "OK");
                    }
                });
            };

            //текст снизу страницы сканирования
            var overlay = new ZXingDefaultOverlay
            {
                TopText = null,
                BottomText = "Автомаическое сканирование каждые 3 сек.",
                ShowFlashButton = true, //кнопка фонарика

            };

            //обработчик кнопки фонарика
            overlay.FlashButtonClicked += (a, b) =>
            {
                zxing.IsTorchOn = !zxing.IsTorchOn;
            };

            //кнопка отмены
            var cancel = new Button()
            {
                Text = "Отмена",
                BackgroundColor = Color.Red,
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            //обработчик кнопки отмены
            cancel.Clicked += (a, b) =>
            {
                Navigation.PopToRootAsync();
                NavigationPage navPage = (NavigationPage)App.Current.MainPage;
            };

            //данная страница постоена на Grid
            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            //добавленик в Grid элементов
            grid.Children.Add(zxing);
            grid.Children.Add(overlay);
            grid.Children.Add(cancel);

            // объявляем элементы на страницу
            Content = grid;
        }
    }
}
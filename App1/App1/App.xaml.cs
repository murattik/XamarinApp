using App1.Repository;
using App1.View.Account;
using App1.View.Rotation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace App1
{
    public partial class App : Application
    {
        public INavigation Navigation { get; set; }
        //Создаваемое подключение к БД будет общим для всего приложения
        public const string DATABASE_NAME = "barecode.db";

        public static BarecodeRepository database;
        public static BarecodeRepository Database
        {
            get
            {
                if (database == null)
                {
                    database = new BarecodeRepository(DATABASE_NAME);
                }
                return database;
            }
        }

        public static MapsGoodsRepository mapsDatabase;
        public static MapsGoodsRepository MapsDatabase
        {
            get
            {
                if (mapsDatabase == null)
                {
                    mapsDatabase = new MapsGoodsRepository(DATABASE_NAME);
                }
                return mapsDatabase;
            }
        }

        public static UsersRepository usersDatabase;
        public static UsersRepository UsersDatabase
        {
            get
            {
                if (usersDatabase == null)
                {
                    usersDatabase = new UsersRepository(DATABASE_NAME);
                }
                return usersDatabase;
            }
        }

        public static UsersOrgAndSkladRepository usersOrgAndSkladDatabase;
        public static UsersOrgAndSkladRepository UsersOrgAndSkladDatabase
        {
            get
            {
                if (usersOrgAndSkladDatabase == null)
                {
                    usersOrgAndSkladDatabase = new UsersOrgAndSkladRepository(DATABASE_NAME);
                }
                return usersOrgAndSkladDatabase;
            }
        }

        public App()
        {
            //var startPage = new PopUpPage();
            //MainPage = new NavigationPage(startPage);
            InitializeComponent();

            //для активации навигации нужно использовать NavigationPage
            //MainPage = new NavigationPage(new LoginPage());
            
            MainPage = new NavigationPage(new RotationListPage())
            {
                BarBackgroundColor = Color.FromHex("#12B812"),
                BarTextColor = Color.White,

                
            };

        }

        protected override void OnStart()
        {
           
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override async void OnResume()
        {
            
            //MainPage = new NavigationPage(new LoginPincode());
            var nav = MainPage.Navigation;

            // you may want to clear the stack (history)
            //await nav.PopToRootAsync(true);

            // then open the needed page (I'm guessing a login page)
            await nav.PushAsync(new LoginPincode());
            //Navigation.PushModalAsync(new LoginPincode());
        }
    }
}

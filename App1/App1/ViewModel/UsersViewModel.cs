using App1.Model;
using App1.View.Account;
using App1.WebService;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModel
{
    class UsersViewModel : INotifyPropertyChanged
    {
        bool initialized = false;   // была ли начальная инициализация
        private bool isBusy;    // идет ли загрузка с сервера

        public ObservableCollection<Users> Users { get; set; } //объявляем коллекцию для вывода пользователю полученных с сервера товаров
        public string Login { get; set; }
        public string Password { get; set; }
        public int PinCode { get; set; }

        LoginService loginService = new LoginService();  //инициализируем сервис
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoginCommand { get; protected set; }
        public ICommand LoginPincodeCommand { get; protected set; }

        public INavigation Navigation { get; set; }

        //загрузчик
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged("IsLoaded");
            }
        }
        public bool IsLoaded
        {
            get { return !isBusy; }
        }

        //конструктор
        public UsersViewModel()
        {
            //создаем коллекцию
            Users = new ObservableCollection<Users>();
            IsBusy = false; //загрузчик не активирован
            ////команды
            LoginCommand = new Command(LoginUser);
            LoginPincodeCommand = new Command(LoginUserPIN);

        }

        private async void LoginUser()
        {
            if (initialized == true) return;
            IsBusy = true;

            var rec = await loginService.LoginAsync(Login, Password);

            if (rec != null)
            {
                List<UserOrgAndSklad> recOrgAndSklad = await loginService.AddSkladAndOrgForUser(rec.MapsUserName);

                foreach (var data in recOrgAndSklad)
                {
                    if (data.NameOrg == null && recOrgAndSklad.Count() == 1)//если пользователю не привязаны клиенты, то просто впускаем его в приложение
                    {
                        IsBusy = false;
                        initialized = true;
                        await Navigation.PushAsync(new MainPage());
                    }
                    else//иначе переходим на страницу выбора клиента и склада
                    {
                        IsBusy = false;
                        initialized = true;
                        await Navigation.PushAsync(new AddOrgForUserPage());
                        break;
                    }
                }

                //очищаем стек сраниц
                var existingPages = Navigation.NavigationStack.ToList();
                foreach (var page in existingPages)
                {
                    Navigation.RemovePage(page);
                }

               
            }

            else
            {
                IsBusy = false;
                initialized = true;
                await Navigation.PushAsync(new ErrorLogin());

                //очищаем стек страниц
                var existingPages = Navigation.NavigationStack.ToList();
                foreach (var page in existingPages)
                {
                    Navigation.RemovePage(page);
                }
            }    
        }

        private async void LoginUserPIN()
        {
            var rec = App.UsersDatabase.GetItems();
            int count = rec.Count();
            int pin = rec.Where(a => a.PinCode == PinCode).Select(a => a.PinCode).FirstOrDefault();
            
            if (rec != null && count == 1 && pin == PinCode && PinCode != 0)
            {
                await this.Navigation.PopAsync();
            }
            else
            {
                await Navigation.PushAsync(new ErrorPincode());

                ////очищаем стек страниц
                //var existingPages = Navigation.NavigationStack.ToList();
                //foreach (var page in existingPages)
                //{
                //    Navigation.RemovePage(page);
                //}
            }
        }

        //вывод товаров с сервиса
        public async Task GetUsers()
        {
            if (initialized == true) return;
            //активируется загрузчик
            IsBusy = true;

            //добавляем данные в коллекцию
            IEnumerable<Users> users = await loginService.Get();
            // очищаем список
            while (Users.Any())
                Users.RemoveAt(Users.Count - 1);

            // добавляем загруженные данные коллекцию
            foreach (Users a in users)
            {

                Users.Add(a);

            }
            //отключаем загрузчик после получения всех данных
            IsBusy = false;
            initialized = true;
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}

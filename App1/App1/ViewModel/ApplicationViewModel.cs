using App1.WebService;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        bool initialized = false;   // была ли начальная инициализация
        tdGoods selectedGoods;  // выбранный товар
        private bool isBusy;    // идет ли загрузка с сервера

        public ObservableCollection<tdGoods> Goods { get; set; } //объявляем коллекцию для вывода пользователю полученных с сервера товаров
        MobileService mobileService = new MobileService();  //инициализируем сервис
        public event PropertyChangedEventHandler PropertyChanged;

        //команды для работы с данными и с сервисом (CRUD использоваться не будет)
        public ICommand CreateGoodsCommand { protected set; get; }
        public ICommand DeleteGoodsCommand { protected set; get; }
        public ICommand SaveGoodsCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }

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
        public ApplicationViewModel()
        {
            //создаем коллекцию
            Goods = new ObservableCollection<tdGoods>();
            IsBusy = false; //загрузчик не активирован
            //команды
            CreateGoodsCommand = new Command(CreateGoods);
            DeleteGoodsCommand = new Command(DeleteGoods);
            SaveGoodsCommand = new Command(SaveGoods);
            BackCommand = new Command(Back);
        }

        //вывод выбранного товара
        public tdGoods SelectedGoods
        {
            get { return selectedGoods; }
            set
            {
                if (selectedGoods != value)
                {
                    tdGoods tmpgoods = new tdGoods()
                    {
                        tdGoodsID = value.tdGoodsID,
                        CodeGoods = value.CodeGoods,
                        CodeID = value.CodeID,
                        BarCode = value.BarCode,
                        Boxes = value.Boxes,
                        Pallet = value.Pallet
                    };
                    selectedGoods = null;
                    OnPropertyChanged("SelectedGoods");
                    Navigation.PushAsync(new GoodsPage(tmpgoods, this));
                }
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        //добавление нового товара
        private async void CreateGoods()
        {
            await Navigation.PushAsync(new GoodsPage(new tdGoods(), this));
        }

        //вернуться на предыдущую страницу
        private void Back()
        {
            Navigation.PopAsync();
        }

        //вывод товаров с сервиса
        public async Task GetGoods()
        {
            if (initialized == true) return;
            //активируется загрузчик
            IsBusy = true;

            //добавляем данные в коллекцию
            IEnumerable<tdGoods> goods = await mobileService.Get();
            // очищаем список
            while ( Goods.Any())
                Goods.RemoveAt(Goods.Count - 1);

            // добавляем загруженные данные коллекцию, где остатки > 0
            foreach (tdGoods a in goods)
            {
                if(a.Boxes > 0 && a.Pallet > 0)
                {
                    Goods.Add(a);
                }  
            }
            //отключаем загрузчик после получения всех данных
            IsBusy = false;
            initialized = true;
        }

        //сохранение товара 
        private async void SaveGoods(object goodsObject)
        {
            tdGoods goods = goodsObject as tdGoods;
            if (goods != null)
            {
                IsBusy = true;
                // редактирование
                if (goods.tdGoodsID > 0)
                {
                    tdGoods updGoods = await mobileService.Update(goods);
                    // заменяем объект в списке на новый
                    if (updGoods != null)
                    {
                        int pos = Goods.IndexOf(updGoods);
                        Goods.RemoveAt(pos);
                        Goods.Insert(pos, updGoods);
                    }
                }
                // добавление
                else
                {
                    tdGoods addedGoods = await mobileService.Add(goods);
                    if (addedGoods != null)
                    {
                        Goods.Add(addedGoods);
                    }
                }
                IsBusy = false;
            }
            Back();
        }

        //удаляем товар
        private async void DeleteGoods(object goodsObject)
        {
            tdGoods goods = goodsObject as tdGoods;
            if (goods != null)
            {
                IsBusy = true;
                tdGoods deletedGoods = await mobileService.Delete(goods.tdGoodsID);
                if (deletedGoods != null)
                {
                    Goods.Remove(deletedGoods);
                }
                IsBusy = false;
            }
            Back();
        }
    }
}

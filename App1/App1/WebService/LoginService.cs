using App1.Model;
using App1.Repository;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.WebService
{
    class LoginService
    {
        //адрес веб-сервиса
        const string Url = "http://192.168.2.52/MobileService/api/Account/";
        HttpClient client = new HttpClient();

        // настройка клиента
        private HttpClient GetClient()
        {
           
            client.DefaultRequestHeaders.Add("Accept", "application/json"); //устанавливаем заголовок Accept для получения json данных от сервиса
            return client;
        }

        // получаем всех пользователей 
        public async Task<IEnumerable<Users>> Get()
        {
            //подключаемся к сервису
            string result = await client.GetStringAsync(Url);

            //вывод данных с сервера
            return JsonConvert.DeserializeObject<IEnumerable<Users>>(result); //преобразование в объект Goods
        }


        public async Task<Users> LoginAsync(string login, string password)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentsPath, "barecode.db");

            //получаем место хранения БД
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(path);
            SQLiteConnection database = new SQLiteConnection(databasePath);

            string response = await client.GetStringAsync(Url + "GetUserLogin/?Login=" + login + "&&Password=" + password);
            
            //пересоздаем локаьную БД 
            database.DropTable<Users>();
            database.CreateTable<Users>();
            database.Insert(JsonConvert.DeserializeObject<Users>(response)); //вставляем данные в таблицу

            return JsonConvert.DeserializeObject<Users>(response);
        }

        public async Task<List<UserOrgAndSklad>> AddSkladAndOrgForUser(string mapsUserName)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentsPath, "barecode.db");

            //получаем место хранения БД
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(path);
            SQLiteConnection database = new SQLiteConnection(databasePath);

            string response = await client.GetStringAsync(Url + "GetOrg/?userName=" + mapsUserName);
            //пересоздаем локаьную БД 
            database.DropTable<UserOrgAndSklad>();
            database.CreateTable<UserOrgAndSklad>();
            
            IEnumerable<UserOrgAndSklad> rec = JsonConvert.DeserializeObject<IEnumerable<UserOrgAndSklad>>(response);
            //string name = rec.Where(a => a.NameOrg != null).Select(a => a.NameOrg).FirstOrDefault();

                foreach (var data in rec)
                {
                    App.UsersOrgAndSkladDatabase.SaveItem(data);
                    //database.Insert(JsonConvert.DeserializeObject<IEnumerable<UserOrgAndSklad>>(response)); //вставляем данные в таблицу
                }


            return JsonConvert.DeserializeObject<List<UserOrgAndSklad>>(response);
        }
    }
}

using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.WebService
{
    class MobileService
    {
        //адрес веб-сервиса
        const string Url = "http://192.168.4.52/MobileService/api/Goods/";

        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json"); //устанавливаем заголовок Accept для получения json данных от сервиса
            return client;
        }

        // получаем все товары 
        public async Task<IEnumerable<tdGoods>> Get()
        {
            //вставляем полученные даныые в локальную БД
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentsPath, "barecode.db");

            //получаем место хранения БД
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(path);
            SQLiteConnection database = new SQLiteConnection(databasePath);

            //подключаемся к сервису
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            
            //пересоздаем локаьную БД для доступа к товарам оффлайн
            database.DropTable<MapsGoods>();
            database.CreateTable<MapsGoods>();
            database.InsertAll(JsonConvert.DeserializeObject<IEnumerable<MapsGoods>>(result)); //вставляем данные в таблицу
           
            //вывод данных с сервера
            return JsonConvert.DeserializeObject<IEnumerable<tdGoods>>(result); //преобразование в объект Goods
        }

                                                              //не будет использоваться
        // добавляем новый товар
        public async Task<tdGoods> Add(tdGoods goods)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(Url,
                new StringContent(
                    JsonConvert.SerializeObject(goods),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<tdGoods>(
                await response.Content.ReadAsStringAsync());
        }

        // обновляем товар
        public async Task<tdGoods> Update(tdGoods goods)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(Url + "/" + goods.tdGoodsID,
                new StringContent(
                    JsonConvert.SerializeObject(goods),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<tdGoods>(
                await response.Content.ReadAsStringAsync());
        }
        // удаляем товар
        public async Task<tdGoods> Delete(int id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + "/" + id);
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<tdGoods>(
               await response.Content.ReadAsStringAsync());
        }
    }
}

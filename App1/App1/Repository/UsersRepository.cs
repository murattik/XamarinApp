using App1.Model;
using SQLite;
using System.Collections.Generic;
using Xamarin.Forms;

namespace App1.Repository
{
    public class UsersRepository
    {
        SQLiteConnection database;

        public UsersRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Users>();
        }

        public IEnumerable<Users> GetItems()
        {
            return database.Table<Users>().ToList();
        }

        public Users GetItem(int id)
        {
            return database.Get<Users>(id);
        }

        public int DeleteItem(int id)
        {
            return database.Delete<Users>(id);
        }

        public int SaveItem(Users item)
        {
            if (item.UserId != 0)
            {
                database.Update(item);
                return item.UserId;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }
}

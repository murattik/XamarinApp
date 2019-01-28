using App1.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Repository
{
    public class UsersOrgAndSkladRepository
    {
        SQLiteConnection database;

        public UsersOrgAndSkladRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            database = new SQLiteConnection(databasePath);
            database.CreateTable<UserOrgAndSklad>();
        }

        public IEnumerable<UserOrgAndSklad> GetItems()
        {
            return database.Table<UserOrgAndSklad>().ToList();
        }

        public UserOrgAndSklad GetItem(int id)
        {
            return database.Get<UserOrgAndSklad>(id);
        }

        public int DeleteItem(int id)
        {
            return database.Delete<UserOrgAndSklad>(id);
        }

        public int SaveItem(UserOrgAndSklad item)
        {
            if (item.ID != 0)
            {
                database.Update(item);
                return item.ID;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }
}

using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace App1
{
    public class BarecodeRepository
    {
        SQLiteConnection database;

        public BarecodeRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            database = new SQLiteConnection(databasePath);
            database.CreateTable<BarecodeResult>();
            //database.Query<BarecodeResult>("INSERT INTO [BarecodeResult] ([BarecodeFormat], [TextResult]) VALUES ('340006617', '4690388003005'), ('555555555', '4444444444444')");
        }

        public IEnumerable<BarecodeResult> GetItems()
        {
            return //database.Table<BarecodeResult>().ToList(); 
            (from i  in database.Table<BarecodeResult>() select i).ToList();
        }

        public BarecodeResult GetItem(int id)
        {
            return database.Get<BarecodeResult>(id);
        }

        public int DeleteItem(int id)
        {
            return database.Delete<BarecodeResult>(id);
        }

        public int SaveItem(BarecodeResult item)
        {
            if (item.Id != 0)
            {
                database.Update(item);
                return item.Id;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Android.Util;
using Android.Widget;
using SQLite;


namespace App3
{
    public class Database
    {
        string Folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public bool CreateDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(Folder, "listitem.db")))
                {
                    connection.CreateTable<ListItem>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool InsertIntoTable(ListItem item)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(Folder, "listitem.db")))
                {
                    connection.Insert(item);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<ListItem> Getallitems()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(Folder, "listitem.db")))
                {
                    return connection.Table<ListItem>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public bool UpdateTable(ListItem item)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(Folder, "listitem.db")))
                {
                    connection.Query<ListItem>("UPDATE Person set Title=?, Subtitle=?, Distance=? , Image=? Where Id=?", item.Title, item.Subtitle, item.Distance, item.Image);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool SelectTable(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(Folder, "listitem.db")))
                {
                    connection.Query<ListItem>("SELECT * FROM ListItem Where Id=?", Id);
                    connection.Delete(Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool DeleteItem(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(Folder, "listitem.db")))
                {

                    connection.Query<ListItem>("Delete  from ListItem where Id=?",Id.ToString());
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
    }
}

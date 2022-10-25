using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsBot
{
    internal class DB
    {
        public static void DelDB()
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=data.db"))
                {
                    connection.Open();
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = connection;
                    command.CommandText = "DROP TABLE feeds";
                    command.ExecuteNonQuery();
                }
            }
            catch
            {

            }
            
        }
        public static void CreateDb()
        {
            using (var connection = new SqliteConnection("Data Source=data.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = "CREATE TABLE feeds(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, link TEXT NOT NULL)";
                command.ExecuteNonQuery();
            }
        }
        public static void Add(string _link)
        {
            using (var connection = new SqliteConnection("Data Source=data.db"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO feeds (link) VALUES (@link)";
                command.Parameters.AddWithValue("@link", _link);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }
        }

        public static bool Contains(string link)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=data.db"))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand();
                    command.Connection = connection;
                    string _command = "SELECT * FROM feeds WHERE [link]=" + link;
                    command.CommandText = "SELECT * FROM feeds WHERE [link]=" + "\"" + link + "\"";
                    //var res = command.ExecuteReader();
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                var _link = reader.GetValue(1);
                                if (link.ToString() == ((string)_link).ToString()) return true;
                                return false;
                            }
                        }
                        return false;
                    }

                }
            }
            catch
            {
                return false;
            }
        }

        public static void Display()
        {
            using (var connection = new SqliteConnection("Data Source=data.db"))
            {
                connection.Open();
                string sqlExpression = "SELECT * FROM feeds";
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данные
                        {
                            var _link = reader.GetValue(1);
                            Console.WriteLine(_link);
                        }
                    }
                }
            }
        }
    }
}

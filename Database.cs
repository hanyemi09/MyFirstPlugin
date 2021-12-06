using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;

namespace MyFirstPlugin
{
    class Database 
    {
        MySqlConnection conn = new MySqlConnection();
        private object server;

        public static List<T> DataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        internal Database Query()
        {
            throw new NotImplementedException();
        }

        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public void Connect(string host, int port, string db, string user, string password)
        {
            conn.ConnectionString = $"server ={ host}; user ={ user}; database ={ db}; port ={ port}; password ={ password}";
            conn.Open();

        }

        public void Disconnect()
        {
            conn.Close();
        }

        public DataTable Query(string query)
        {

            DataTable results = new DataTable();

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                results.Load(cmd.ExecuteReader());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return results;
        }

        public DataTable Update(string query)
        {
            DataTable results = new DataTable();

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();
                }
                while (reader.Read())
                {
                    Console.WriteLine(reader[0] + ", " + reader[1]);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return results;
        }
    }
}

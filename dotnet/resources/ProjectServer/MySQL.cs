using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using GTANetworkAPI;
using System.Threading.Tasks;

namespace ProjectServer
{
    class MySQL
    {
        public static MySqlConnection connection;
        private static string server;
        private static string database;
        private static string uid;
        private static string password;

        public MySQL()
        {
            
        }

        public static bool Initialize()
        {
            server = "localhost";
            database = "rage";
            uid = "root";
            password = "11042001";

            string connectionString;
            connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            connection = new MySqlConnection(connectionString);

            connection.Open();

            return(connection.State == System.Data.ConnectionState.Open);
        }
    }
}

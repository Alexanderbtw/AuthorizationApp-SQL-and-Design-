using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace PostgresConnectionLib
{
    public class DBConnect
    {
        private static Configuration? cur_config;

        public static NpgsqlConnection? Connection { get; private set; }

        public static async void Connect(string? filename = null)
        {
            if (filename == null) cur_config = new Configuration();
            else cur_config = new Configuration(filename);

            Connection = new NpgsqlConnection(cur_config.GetConnectionString());
            await Connection.OpenAsync();
        }

        public static async void Disconnect()
        {
            if (Connection != null)
            {
                await Connection.CloseAsync();
            }
        }

        public static NpgsqlCommand GetCommand(string sql)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(sql);
            cmd.Connection = Connection;
            return cmd;
        }
    }
}

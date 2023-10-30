using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;

namespace PostgresConnectionLib
{
    public class Configuration
    {
        public static event Action<string> ErrorLog;
        private string? filename;

        // IP - 127.0.0.1 if localhost
        private string? Host { get; set; }

        private string? Port { get; set; }

        private string? User { get; set; }

        private string? Pass { get; set; }

        private string? Base { get; set; }

        [JsonConstructor]
        public Configuration(string Host, string Port, string User, string Pass, string Base)
        {
            this.Host = Host;
            this.Port = Port;
            this.User = User;
            this.Pass = Pass;
            this.Base = Base;
        }

        internal Configuration(string filename = "app.json") 
        {
            this.filename = filename;

            try
            {
                FileStream file = File.OpenRead(filename);

                byte[] bytes = new byte[file.Length];
                int bytesRead = file.Read(bytes, 0, bytes.Length);
                file.Close();

                if (bytesRead == 0 || bytesRead != bytes.Length)
                {
                    throw new FileNotFoundException("File is empty!");
                }

                Configuration? conf = JsonConvert.DeserializeObject<Configuration>(Encoding.Default.GetString(bytes));
                if (conf != null)
                {
                    Host = conf.Host;
                    Port = conf.Port;
                    User = conf.User;
                    Pass = conf.Pass;
                    Base = conf.Base;
                }
                else
                {
                    ErrorLog?.Invoke("Not correct file configuration!");
                }
            }
            catch (FileNotFoundException e)
            {
                ErrorLog?.Invoke("File not exist!\nDefault file was created.");
                File.Create(filename).Close();
                Default();
                Save();
            }
            catch (Exception ex)
            {
                ErrorLog?.Invoke(ex.Message);
            }
        }

        private void Default()
        {
            // Your default values
            Host = "127.0.0.1";
            Port = "5432";
            User = "postgres";
            Pass = "Samara2752777";
            Base = "authorize_wpf";
        }

        internal void Save()
        {
            try
            {
                string jsonObj = JsonConvert.SerializeObject(this, Formatting.Indented);

                FileStream file = File.OpenWrite(filename);
                file.Write(Encoding.Default.GetBytes(jsonObj));
                file.Close();
            }
            catch (Exception ex)
            {
                ErrorLog?.Invoke(ex.Message);
            }
        }

        internal string GetConnectionString()
        {
            return $"Server={Host};Port={Port};User Id={User};Password={Pass};Database={Base}";
        }
    }
}

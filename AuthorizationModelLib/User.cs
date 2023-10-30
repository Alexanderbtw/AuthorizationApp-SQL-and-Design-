using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostgresConnectionLib;
using Npgsql;
using NpgsqlTypes;
using System.Collections.ObjectModel;
using System.Data;

namespace AuthorizationModelLib
{
    public class User
    {
        public static event Action<string>? ErrorLog;

        public int user_id { get; set; }

        public string login { get; set; }

        public string pass { get; set; }

        public string email { get; set; }

        public string? birth_date { get; set; }

        public string user_name { get; set; }

        public User() 
        {
            login = string.Empty;
            pass = string.Empty;
            email = string.Empty;
        }

        /*
        public User(string login, string pass, string email)
        {
            this.login = login;
            this.pass = pass;
            this.email = email;
        }
        */

        public bool Create()
        {
            var cmd = DBConnect.GetCommand("INSERT INTO public.users (login, pass, email) VALUES (@login, @pass, @email)");

            cmd.Parameters.AddWithValue("@login", NpgsqlDbType.Varchar, login);
            cmd.Parameters.AddWithValue("@pass", NpgsqlDbType.Varchar, pass);
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Varchar, email);

            int res = 0;

            try
            {
                res = cmd.ExecuteNonQuery();
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res == 1;
        }

        public void Alter()
        {
            var cmd = DBConnect.GetCommand("UPDATE public.users SET login = @login, pass = @pass, email = @email, birth_date = @birth_date, user_name = @user_name WHERE user_id = @user_id");

            cmd.Parameters.AddWithValue("@login", NpgsqlDbType.Varchar, login);
            cmd.Parameters.AddWithValue("@pass", NpgsqlDbType.Varchar, pass);
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Varchar, email);
            cmd.Parameters.AddWithValue("@birth_date", NpgsqlDbType.Date, DateTime.TryParse(birth_date, out DateTime date) ? date : DBNull.Value);
            cmd.Parameters.AddWithValue("@user_name", NpgsqlDbType.Varchar, user_name);
            cmd.Parameters.AddWithValue("user_id", NpgsqlDbType.Integer, user_id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorLog?.Invoke(ex.Message);
            }
        }

        public static void GetCollection(ref ObservableCollection<User> users)
        {
            var cmd = DBConnect.GetCommand("SELECT * FROM public.users ORDER BY user_id");

            var res = cmd.ExecuteReader();

            if (res == null) return; // Можно вызывать исключение или эвент

            if (res.HasRows)
            {
                users.Clear();

                while (res.Read())
                {
                    users.Add(new User()
                    {
                        user_id = res.GetInt32(0),
                        login = res.GetString(1),
                        pass = res.GetString(2),
                        email = res.GetString(3),
                        user_name = res.GetString(4),
                        birth_date = res.IsDBNull(5) ? "Not Indicated" : res.GetDateTime(5).ToShortDateString()
                    });
                }
            }

            res.Close();
        }
    }
}

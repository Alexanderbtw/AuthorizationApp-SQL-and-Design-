using NpgsqlTypes;
using PostgresConnectionLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationModelLib
{
    public class Post
    {
        public long post_id { get; set; }

        public string post_content { get; set; }

        public int fk_user_id { get; set; }

        public DateTime post_date { set; get; }

        public Post() { }

        public Post(int user_id)
        {
            post_content = string.Empty;
            fk_user_id = user_id;
        }

        public void Create()
        {
            var cmd = DBConnect.GetCommand("INSERT INTO public.posts (post_content, fk_user_id) VALUES (@post_content, @user_id)");

            cmd.Parameters.AddWithValue("@post_content", NpgsqlDbType.Text, post_content);
            cmd.Parameters.AddWithValue("@user_id", NpgsqlDbType.Bigint, fk_user_id);

            cmd.ExecuteNonQuery();
        }

        public static void GetCollection(ref ObservableCollection<Post> posts)
        {
            var cmd = DBConnect.GetCommand("SELECT * FROM public.posts ORDER BY post_id");

            var res = cmd.ExecuteReader();

            if (res == null) return;

            if (res.HasRows)
            {
                posts.Clear();

                while (res.Read())
                {
                    posts.Add(new Post()
                    {
                        post_id = res.GetInt64(0),
                        post_content = res.GetString(1),
                        fk_user_id = res.GetInt32(2),
                        post_date = res.GetDateTime(3)

                    });
                }
            }

            res.Close();
        }
    }
}

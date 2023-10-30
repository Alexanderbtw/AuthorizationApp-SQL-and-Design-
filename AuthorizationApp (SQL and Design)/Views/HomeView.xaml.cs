using AuthorizationApp__SQL_and_Design_.Commands;
using AuthorizationModelLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AuthorizationApp__SQL_and_Design_.Views
{
    /// <summary>
    /// Логика взаимодействия для HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public Post cur_post { get; set; }


        public HomeView()
        {
            cur_post = new Post(PersonalAreaWindow.cur_user.user_id);

            InitializeComponent();
        }

        private void btnPostPublish_Click(object sender, RoutedEventArgs e)
        {
            cur_post.Create();

            tbPost.Text = String.Empty;
            cur_post = new Post(PersonalAreaWindow.cur_user.user_id);
            tbPost.GetBindingExpression(DataContextProperty)?.UpdateTarget();
            PersonalAreaWindow.UpdatePostsCollection();
            lbPosts.ItemsSource = PersonalAreaWindow.AllPosts;
        }
    }
}

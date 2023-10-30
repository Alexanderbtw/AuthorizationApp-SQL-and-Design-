using AuthorizationApp__SQL_and_Design_.ViewModels;
using AuthorizationModelLib;
using CustomDesignWinLib;
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
using System.Windows.Shapes;

namespace AuthorizationApp__SQL_and_Design_
{
    /// <summary>
    /// Логика взаимодействия для PersonalAreaWindow.xaml
    /// </summary>
    public partial class PersonalAreaWindow : Window
    {
        private static ObservableCollection<Post> all_posts = new ObservableCollection<Post>();
        public static ObservableCollection<Post> AllPosts
        {
            get { return all_posts; }
            set { all_posts = value; }
        }

        public static User cur_user { get; set; }

        public PersonalAreaWindow(User user)
        {
            cur_user = user;
            UpdatePostsCollection();
            InitializeComponent();
            User.ErrorLog += LogHandler;
            DataContext = new MainViewModel();
        }

        public static void UpdatePostsCollection()
        {
            Post.GetCollection(ref all_posts);
        }

        private void LogHandler(string mes)
        {
            new CustomMessageBox(mes, MessageType.Warning, MessageButtons.Ok).ShowDialog();
        }
    }
}

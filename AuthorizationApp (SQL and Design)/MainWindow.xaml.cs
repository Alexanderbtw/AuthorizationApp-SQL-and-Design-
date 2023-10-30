using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
using PostgresConnectionLib;
using CustomDesignWinLib;
using AuthorizationModelLib;
using System.Collections.ObjectModel;
using System.Collections;

namespace AuthorizationApp__SQL_and_Design_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User NewUser { get; set; }
        public ObservableCollection<User> Users = new ObservableCollection<User>();

        public MainWindow()
        {
            NewUser = new User();
            InitializeComponent();

            Configuration.ErrorLog += LogHandler;
            DBConnect.Connect();

            //DataContext = this;
        }

        private void LoadUsers()
        {
            User.GetCollection(ref Users);
        }

        private void LogHandler(string mes)
        {
            new CustomMessageBox(mes, MessageType.Error, MessageButtons.Ok).ShowDialog();
        }

        private async void btnReg_Click(object sender, RoutedEventArgs e)
        {
            string pass2 = pbPasswordRep.Password;

            if (!NewUser.email.Contains('@') || !NewUser.email.Contains('.'))
            {
                HintAssist.SetHint(tbEmail, "Uncorrect email");
                HintAssist.SetForeground(tbEmail, Brushes.Red);
                tbEmail.Foreground = Brushes.Red;
                tbEmail.BorderBrush = Brushes.Red;
                tbEmail.Focus();
                
            }
            else if (NewUser.login.Length < 5)
            {
                HintAssist.SetHint(tbLogin, "Login is too short");
                HintAssist.SetForeground(tbLogin, Brushes.Red);
                tbLogin.Foreground = Brushes.Red;
                tbLogin.BorderBrush = Brushes.Red;
                tbLogin.Focus();
            }
            else if (NewUser.pass.Length < 5)
            {
                HintAssist.SetHint(pbPassword, "Password is too easy");
                HintAssist.SetForeground(pbPassword, Brushes.Red);
                pbPassword.Foreground = Brushes.Red;
                pbPassword.BorderBrush = Brushes.Red;
                pbPassword.Focus();
            }
            else if (NewUser.pass != pass2)
            {
                HintAssist.SetHint(pbPasswordRep, "Unother password");
                HintAssist.SetForeground(pbPasswordRep, Brushes.Red);
                pbPasswordRep.Foreground = Brushes.Red;
                pbPasswordRep.BorderBrush = Brushes.Red;
                pbPasswordRep.Focus();
            }
            else
            {
                if (!NewUser.Create())
                {
                    SnackNotion("Login or email already exist");
                    return;
                }

                txtMain.Text += " successful";
                txtMain.Foreground = Brushes.LawnGreen;
                UserPanel.Visibility = Visibility.Collapsed;
                LoadCircle.Visibility = Visibility.Visible;
                btnRegAuth.IsEnabled = false;
                ButtonProgressAssist.SetIsIndicatorVisible(btnRegAuth, true);

                await Task.Delay(5000);

                new MainWindow().Show();
                this.Close();

                NewUser = new User();
                UserPanel.GetBindingExpression(DataContextProperty)?.UpdateTarget();
            }
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();

            if (!Users.Where(x => (x.login == NewUser.login) && (x.pass == NewUser.pass)).Any())
            {
                txtMain.Text = "Authorization Failed";
                txtMain.Foreground = Brushes.Red;
                return;
            }

            new PersonalAreaWindow(Users.First(x => x.login == NewUser.login)).Show();
            this.Close();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            btnSignIn.Style = Application.Current.Resources["MaterialDesignRaisedButton"] as Style;
            btnSignUp.Style = Application.Current.Resources["MaterialDesignOutlinedButton"] as Style;

            txtMain.Text = "Authorization";

            tbEmail.Visibility = Visibility.Collapsed;
            pbPasswordRep.Visibility = Visibility.Collapsed;

            btnRegAuth.Click -= btnReg_Click;
            btnRegAuth.Click += btnAuth_Click;
            btnRegAuth.Content = "Authorize";
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            btnSignIn.Style = Application.Current.Resources["MaterialDesignOutlinedButton"] as Style;
            btnSignUp.Style = Application.Current.Resources["MaterialDesignRaisedButton"] as Style;

            txtMain.Text = "Registration";

            tbEmail.Visibility = Visibility.Visible;
            pbPasswordRep.Visibility = Visibility.Visible;

            btnRegAuth.Click += btnReg_Click;
            btnRegAuth.Click -= btnAuth_Click;
            btnRegAuth.Content = "Register";
        }

        private async void SnackNotion(string mes)
        {
            sbLog.IsActive = true;
            sbLog.MessageQueue?.Enqueue(mes);
            await Task.Delay(3000);
            sbLog.IsActive = false;
        }
    }
}

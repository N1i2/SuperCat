using SuperCat.MyObjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SuperCat.Context;
using System.Windows.Media;
using SuperCat.Lists;

namespace SuperCat.Log
{
    /// <summary>
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
        private CreateAccount create = new CreateAccount();
        private List<UserInfo> users = new List<UserInfo>();

        public LogPage()
        {
            InitializeComponent();
        }
        private string GetPassword()
        {
            using (var context = new SuperCatContext())
            {
                users = context.UsersInfo.Where(us => us.Nikname == niknameBox.Text).ToList();
            }

            return (users.Count == 0) ? "" : users[0].Password;
        }
        private UserInfo GetId()
        {
            using (var context = new SuperCatContext())
            {
                return context.UsersInfo.Where(ur => ur.Nikname.Equals(niknameBox.Text)).ToList().First();
            }
        }
        private void LogInAccount(object sender, RoutedEventArgs e)
        {
            try
            {
                string pass = GetPassword();

                if (pass == "")
                    throw new Exception("Takih net");
                if (!pass.Equals(passwordBox.Password))
                    throw new Exception("Ne tot parol");

                NavigationService.Navigate(new MyList(GetId()));
            }
            catch
            {
                ErrorLog.Visibility = Visibility.Visible;
            }
        }

        private void CreateNewAccounts(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(create);
        }

        private void ShowPass(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            liyPasswordBox.Text = passwordBox.Password;
            liyPasswordBox.Visibility = Visibility.Visible;
        }

        private void UnshowPass(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            liyPasswordBox.Visibility = Visibility.Hidden;
        }
    }
}

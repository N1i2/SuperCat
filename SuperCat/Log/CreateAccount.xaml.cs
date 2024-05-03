using SuperCat.Context;
using SuperCat.MyObjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace SuperCat.Log
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Page
    {
        private bool nicknameWell = false;
        private bool passwordWell = false;
        private bool nameWell = false;
        private EndCreateAccount create = new EndCreateAccount();
        private List<UserInfo> arr = new List<UserInfo>();

        public CreateAccount()
        {
            InitializeComponent();
            GetUsers();
        }

        private void GetUsers()
        {
            using (var context = new SuperCatContext())
            {
                arr = context.UsersInfo.ToList();
            }
        }
        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void LostFocusNikname(object sender, RoutedEventArgs e)
        {
            foreach (var user in arr)
            {
                bool end = true;

                if (user.Nikname == NiknameBox.Text)
                    end = false;

                if (end)
                {
                    NiknameBox.Background = Brushes.White;
                    if (NiknameBox.Text.Replace(" ", "").Length > 0)
                        nicknameWell = true;
                    else
                        nicknameWell = false;

                    ErrorBox.Visibility = Visibility.Hidden;
                    return;
                }

                if (NiknameBox.Text.Replace(" ", "").Length > 0)
                    nicknameWell = false;
            }

            ErrorBox.Content = Application.Current.Resources["NiknameError"] as string;
            ErrorBox.Visibility = Visibility.Visible;
            ShowError((TextBox)sender);
        }
        private void LostFocusPassword(object sender, RoutedEventArgs e)
        {
            var txt = (TextBox)sender;
            string password = txt.Text;
            bool charBool = false;
            bool numbBool = false;

            if (PasswordBox.Text.Length <= 0)
            {
                passwordWell = false;
                return;
            }

            if (PasswordBox.Text.Length >= 4 && PasswordBox.Text.Length <= 15)
            {
                foreach (char c in PasswordBox.Text)
                {
                    if (charBool && numbBool)
                    {
                        break;
                    }

                    if (int.TryParse(Convert.ToString(c), out int numb))
                    {
                        numbBool = true;
                    }
                    else
                    {
                        charBool = true;
                    }
                }

                if (charBool && numbBool)
                {
                    txt.Background = Brushes.White;
                    passwordWell = true;
                    ErrorBox.Visibility = Visibility.Hidden;
                    return;
                }
            }

            passwordWell = false;


            if (PasswordBox.Text.Length < 4)
            {
                ErrorBox.Content = Application.Current.Resources["SmallPasswordError"] as string;
            }
            else if (PasswordBox.Text.Length > 15)
            {
                ErrorBox.Content = Application.Current.Resources["BigPasswordError"] as string;
            }
            else
            {
                ErrorBox.Content = Application.Current.Resources["PasswordError"] as string;
            }

            ErrorBox.Visibility = Visibility.Visible;
            ShowError((TextBox)sender);
        }
        private void LostFocusName(object sender, RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            try
            {
                if (RealNameBox.Text.Length <= 0)
                {
                    nameWell = false;
                    return;
                }


                if (txt.Text.Replace(" ", "").Length < 3)
                    throw new Exception("s");
                if (txt.Text.Replace(" ", "").Length > 60)
                    throw new Exception("b");
                foreach(var sim in txt.Text)
                {
                    if (int.TryParse(Convert.ToString(sim), out int numb))
                        throw new Exception("n");
                }

                ErrorBox.Visibility = Visibility.Hidden;
                txt.Background = Brushes.White;
                nameWell = true;
                return;
            }
            catch(Exception ex)
            {
                if(ex.Message== "s")
                {
                    ErrorBox.Content = Application.Current.Resources["SmallRealNameError"] as string;
                }
                else if(ex.Message == "b")
                {
                    ErrorBox.Content = Application.Current.Resources["BigRealNameError"] as string;
                }
                else if(ex.Message == "n")
                {
                    ErrorBox.Content = Application.Current.Resources["NumberRealNameError"] as string;
                }

                ErrorBox.Visibility = Visibility.Visible;
                nameWell = false;
                ShowError(txt);
            }
        }
        private void ShowError(TextBox sender)
        {
            ColorAnimation animation = new ColorAnimation
            {
                To = Color.FromRgb(200, 100, 100),
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            SolidColorBrush brush = new SolidColorBrush(Colors.White);
            brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            sender.Background = brush;
        }


        private void NiknameBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (NiknameBox.Text.Replace(" ", "").Length > 0)
                PasswordBox.Focus();
        }

        private void PasswordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (PasswordBox.Text.Replace(" ", "").Length > 0)
                RealNameBox.Focus();
        }

        private void RealNameBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (RealNameBox.Text.Replace(" ", "").Length > 0 && nameWell)
            {
                NextButt.Focus();
                NextClick(sender, e);
            }
            else
            {
                LostFocusName(sender, e);
            }
        }
        private void NextClick(object sender, RoutedEventArgs e)
        {
            if (!(nicknameWell && passwordWell && nameWell))
            {
                if (!nicknameWell)
                {
                    ShowError(NiknameBox);
                    NiknameBox.Focus();
                    ErrorBox.Content = Application.Current.Resources["ProblemWithNikname"] as string;
                }
                else if (!passwordWell)
                {
                    ShowError(PasswordBox);
                    PasswordBox.Focus();
                    ErrorBox.Content = Application.Current.Resources["ProblemWithPassword"] as string;
                }
                else if (!nameWell)
                {
                    ShowError(RealNameBox);
                    RealNameBox.Focus();
                    ErrorBox.Content = Application.Current.Resources["ProblemWithRealName"] as string;
                }

                ErrorBox.Visibility = Visibility.Visible;

                return;
            }

            ErrorBox.Visibility = Visibility.Hidden;
            create.Nikname = NiknameBox.Text;
            create.Password = PasswordBox.Text;
            create.RealName = RealNameBox.Text;

            NavigationService.Navigate(create);
        }
    }
}
using SuperCat.Context;
using SuperCat.GlobalFanc;
using SuperCat.MyObjects;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;

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
        private bool GoBack = false;
        private string password = string.Empty;
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
            if (NiknameBox.Text.Replace(" ", "").Length <= 0)
            {
                nicknameWell = false;
                return;
            }
            foreach (var user in arr)
            {
                if (user.Nikname == NiknameBox.Text)
                {
                    ErrorBox.Content = System.Windows.Application.Current.Resources["NiknameError"] as string;
                    ErrorBox.Visibility = Visibility.Visible;
                    nicknameWell = false;
                    ShowError((TextBox)sender);
                    return;
                }
            }

            NiknameBox.Background = null;
            nicknameWell = true;
            ErrorBox.Visibility = Visibility.Hidden;
        }
        private void LostFocusPassword(object sender, RoutedEventArgs e)
        {
            var txt = (PasswordBox)sender;
            string password = txt.Password;
            bool charBool = false;
            bool numbBool = false;

            if (password.Length <= 0)
            {
                passwordWell = false;
                return;
            }

            if (password.Length >= 4 && password.Length <= 15)
            {
                foreach (char c in password)
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
                    txt.Background = null;
                    passwordWell = true;
                    ErrorBox.Visibility = Visibility.Hidden;
                    return;
                }
            }

            passwordWell = false;


            if (password.Length < 4)
            {
                ErrorBox.Content = System.Windows.Application.Current.Resources["SmallPasswordError"] as string;
            }
            else if (password.Length > 15)
            {
                ErrorBox.Content = System.Windows.Application.Current.Resources["BigPasswordError"] as string;
            }
            else
            {
                ErrorBox.Content = System.Windows.Application.Current.Resources["PasswordError"] as string;
            }

            ErrorBox.Visibility = Visibility.Visible;
            ShowError((PasswordBox)sender);
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
                txt.Background = null;
                nameWell = true;
                return;
            }
            catch(Exception ex)
            {
                if(ex.Message== "s")
                {
                    ErrorBox.Content = System.Windows.Application.Current.Resources["SmallRealNameError"] as string;
                }
                else if(ex.Message == "b")
                {
                    ErrorBox.Content = System.Windows.Application.Current.Resources["BigRealNameError"] as string;
                }
                else if(ex.Message == "n")
                {
                    ErrorBox.Content = System.Windows.Application.Current.Resources["NumberRealNameError"] as string;
                }

                ErrorBox.Visibility = Visibility.Visible;
                nameWell = false;
                ShowError(txt);
            }
        }
        private void ShowError(Control sender)
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
                passwordBox.Focus();
        }

        private void PasswordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            if (passwordBox.Password.Replace(" ", "").Length > 0)
                RealNameBox.Focus();
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
            if (!(nicknameWell 
                //commet password line
                && passwordWell
                && nameWell))
            {
                if (!nicknameWell)
                {
                    ShowError(NiknameBox);
                    NiknameBox.Focus();
                    ErrorBox.Content = System.Windows.Application.Current.Resources["ProblemWithNikname"] as string;
                }
                else if (!passwordWell)
                {
                    ShowError(passwordBox);
                    passwordBox.Focus();
                    ErrorBox.Content = System.Windows.Application.Current.Resources["ProblemWithPassword"] as string;
                }
                else if (!nameWell)
                {
                    ShowError(RealNameBox);
                    RealNameBox.Focus();
                    ErrorBox.Content = System.Windows.Application.Current.Resources["ProblemWithRealName"] as string;
                }

                ErrorBox.Visibility = Visibility.Visible;

                return;
            }

            if (!GoBack)
            {
                GoBack = !GoBack;
                password = passwordBox.Password;
            }

            ErrorBox.Visibility = Visibility.Hidden;
            create.Nikname = NiknameBox.Text;
            create.RealName = RealNameBox.Text;

            create.Password = HelpWork.HashPassword(passwordBox.Password);

            NavigationService.Navigate(create);
        }

        private void passwordBox_Loaded(object sender, RoutedEventArgs e)
        {
            if(GoBack)
            {
                passwordBox.Password = password;
            }
        }
    }
}
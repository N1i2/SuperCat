using SuperCat.Context;
using SuperCat.MyObjects;
using SuperCat.Lists;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.IO;
using System.Text.Json;

namespace SuperCat.Log
{
    /// <summary>
    /// Interaction logic for EndCreateAccount.xaml
    /// </summary>
    public partial class EndCreateAccount : Page
    {
        public string Nikname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RealName { get; set; } = string.Empty;

        private int currentYear;
        private int currentMonth;
        private int currentDay;
        private int nowYear;
        private int nowMonth;
        private int nowDay;

        public EndCreateAccount()
        {
            InitializeComponent();


            LoadedStartData();
            dateNotNeed(0, new RoutedEventArgs());
            GetYear();
        }

        private void LoadedStartData()
        {
            var time = DateTime.Now;

            currentYear = time.Year;
            nowYear = 0;
            currentMonth = time.Month;
            nowMonth = 0;
            currentDay = time.Day;
            nowDay = 0;
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }


        private void dateNeed(object sender, RoutedEventArgs e)
        {
            LiyDay.Visibility = Visibility.Hidden;
            LiyMonth.Visibility = Visibility.Hidden;
            LiyYear.Visibility = Visibility.Hidden;

            YourDay.Visibility = Visibility.Visible;
            YourMonth.Visibility = Visibility.Visible;
            YourYear.Visibility = Visibility.Visible;
        }

        private void dateNotNeed(object sender, RoutedEventArgs e)
        {
            LiyDay.Visibility = Visibility.Visible;
            LiyMonth.Visibility = Visibility.Visible;
            LiyYear.Visibility = Visibility.Visible;

            YourDay.Visibility = Visibility.Hidden;
            YourMonth.Visibility = Visibility.Hidden;
            YourYear.Visibility = Visibility.Hidden;
        }

        private void GetYear()
        {
            for (int i = currentYear; i >= currentYear - 150; i--)
            {
                YourYear.Items.Add(i);
            }

            YourYear.SelectedIndex = nowYear;
        }
        private void GetMonth(bool all = false)
        {
            YourMonth.Items.Clear();

            var MonthName = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames;

            int step = (all) ? 12 : currentMonth;

            for (int i = 0; i < step; i++)
            {
                YourMonth.Items.Add(MonthName[i]);
            }

            YourMonth.SelectedIndex = nowMonth;
        }
        private void GetDay(bool all = false)
        {
            YourDay.Items.Clear();

            int step = (all) ? DateTime.DaysInMonth(Convert.ToInt32(YourYear.Items[nowYear].ToString()), nowMonth + 1) : currentDay;

            for (int i = 0; i < step; i++)
            {
                YourDay.Items.Add(i + 1);
            }

            YourDay.SelectedIndex = nowDay;
        }
        private void SelectionChangedYear(object sender, SelectionChangedEventArgs e)
        {
            nowYear = YourYear.SelectedIndex;

            if (nowYear == 0)
            {
                nowMonth = currentMonth - 1;
                GetMonth();

                return;
            }

            nowMonth = YourMonth.SelectedIndex;
            GetMonth(true);
        }
        private void SelectionChangedMonth(object sender, SelectionChangedEventArgs e)
        {
            int help = YourMonth.SelectedIndex;
            if (help != -1)
                nowMonth = help;

            if (nowMonth == currentMonth - 1 && nowYear == 0)
            {
                nowDay = currentDay - 1;
                GetDay();

                return;
            }

            nowDay = 0;
            GetDay(true);
        }

        private void SelectionChangedDay(object sender, SelectionChangedEventArgs e)
        {
            int help = YourDay.SelectedIndex;
            if (help != -1)
                nowDay = help;
        }

        private void needGend(object sender, RoutedEventArgs e)
        {
            LiyMan.Visibility = Visibility.Hidden;
            LiyWoman.Visibility = Visibility.Hidden;

            radioMan.Visibility = Visibility.Visible;
            radioWoman.Visibility = Visibility.Visible;
        }

        private void notNeedGend(object sender, RoutedEventArgs e)
        {
            LiyMan.Visibility = Visibility.Visible;
            LiyWoman.Visibility = Visibility.Visible;

            radioMan.Visibility = Visibility.Hidden;
            radioWoman.Visibility = Visibility.Hidden;
        }

        private void needEmail(object sender, RoutedEventArgs e)
        {
            LiyEmail.Visibility = Visibility.Hidden;

            EmailBorder.Visibility = Visibility.Visible;
        }

        private void NotNeedEmail(object sender, RoutedEventArgs e)
        {
            LiyEmail.Visibility = Visibility.Visible;

            EmailBorder.Visibility = Visibility.Hidden;
        }
        private void CreateClick(object sender, RoutedEventArgs e)
        {
            ErreoEmail.Visibility = Visibility.Hidden;
            var userinfo = new UserInfo(Nikname, Password, RealName);

            if (needEmailCheck.IsChecked == true)
            {
                var emailRegex = new Regex(@"^[a-zA-Z0-9]+@[a-zA-Z0-9]{3,5}\.[a-zA-Z0-9]{2,3}$");
                var email = EmailBox.Text;

                if (!emailRegex.IsMatch(email))
                {
                    ErreoEmail.Visibility = Visibility.Visible;
                    EmailBox.Focus();
                    return;
                }

                userinfo.Email = email;
            }
            if (needDateCheck.IsChecked == true)
            {
                userinfo.Birthday = YourYear.SelectedValue.ToString() + "-" +
                    ((YourMonth.SelectedIndex + 1).ToString().Length == 1 ? "0" : "") +
                    (YourMonth.SelectedIndex + 1) + "-" +
                    ((YourDay.SelectedValue.ToString() ?? "").Length == 1 ? "0" : "") + YourDay.SelectedValue.ToString();
            }

            if (needGendCheck.IsChecked == true)
            {
                userinfo.Gender = (radioMan.IsChecked == true) ? "m" : "w";
            }

            var icon = new DefoultImage();

            userinfo.Image = icon.img[Convert.ToInt32(new Random().Next(4))];

            NavigationService.Navigate(new MyList(SaveInfo(userinfo)));
        }

        private UserInfo SaveInfo(UserInfo userInfo)
        {
            using (var context = new SuperCatContext())
            {
                context.UsersInfo.Add(userInfo);

                context.SaveChanges();

                return context.UsersInfo.Where(us => us.Nikname.Equals(userInfo.Nikname)).ToList().First();
            }
        }
    }
}

using SuperCat.Context;
using SuperCat.MyObjects;
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
using SuperCat.Log;
using Microsoft.Win32;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using SuperCat.Paterns;
using System.Drawing;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using SuperCat.GlobalFanc;
using SuperCat.Pages.FriendFile;
using MaterialDesignThemes.Wpf;
using Microsoft.VisualBasic.ApplicationServices;
using System.Security.RightsManagement;

namespace SuperCat.Lists
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        private UserInfo user;
        private bool change = false;
        private MyUnitOfWork work = new MyUnitOfWork(new SuperCatContext());
        private DefoultImage defImg = new DefoultImage();
        private int indexImg = -1;
        Regex emailRegex = new Regex(@"^[a-zA-Z0-9]+@[a-zA-Z0-9]{3,5}\.[a-zA-Z0-9]{2,3}$");
        private bool admin;

        public Settings()
        {
            InitializeComponent();
            user = new UserInfo();
        }
        public Settings(UserInfo user, bool admin = false) : this()
        {
            FillArea(user);
            this.admin = admin;
            GetGender();
            GetYear();
        }

        void GetGender()
        {
            if (user.Gender == "m")
                radioMan.IsChecked = true;
            else if (user.Gender == "w")
                radioWoman.IsChecked = true;
            else
                radioNoGender.IsChecked = true;
        }

        private void FillArea(UserInfo user)
        {
            this.user = user;
            NiknameBox.Content = user.Nikname;
            RealNameBox.Content = user.RealName;
            if (user.Birthday != null)
                BirthdayBox.Content = user.Birthday;
            if (user.Gender == "m")
                GenderBox.Content = System.Windows.Application.Current.Resources["Man"] as string;
            else if (user.Gender == "w")
                GenderBox.Content = System.Windows.Application.Current.Resources["Woman"] as string;
            if (user.Email != null)
            {
                EmailBox.Content = EmailBoxText.Text = user.Email;
            }
            for (int i = 0; i < 10; i++)
            {
                NumberID.Content = user.Id.ToString("D10");
            }
        }
        private void DoChange(Label block, TextBox box, Button button)
        {
            button.Visibility = Visibility.Visible;
            block.Visibility = Visibility.Hidden;

            if (box != null)
            {
                box.Text = block.Content.ToString();

                box.Visibility = Visibility.Visible;
                box.Focus();
                box.CaretIndex = int.MaxValue;
            }
        }
        private void DoSave(Label block, TextBox box, Button button)
        {
            change = true;

            work.userRepository.Update(user);
            work.Save();

            button.Visibility = Visibility.Visible;
            block.Visibility = Visibility.Visible;

            if (box != null)
            {
                block.Content = box.Text;
                box.Visibility = Visibility.Hidden;
            }
        }
        private void ChangeNikname_Click(object sender, RoutedEventArgs e)
        {
            ChangeNikname.Visibility = Visibility.Hidden;

            DoChange(NiknameBox, NiknameBoxText, saveNikname);
        }
        private void SaveNikname_Click(object sender, RoutedEventArgs e)
        {
            if (NiknameBoxText.Text.Replace(" ", "").Length == 0)
                return;
            if (NiknameBoxText.Text.Length > 30)
            {
                MessageBox.Show("This realy long Nickname");
                return;
            }
            using (var context = new SuperCatContext())
            {
                List<UserInfo> list = context.UsersInfo.Where(x => x.Nikname.Equals(NiknameBoxText.Text)).ToList();
                if (list.Count >= 1)
                {
                    try
                    {
                        if (list.Count >= 2)
                        {
                            throw new Exception();
                        }

                        if (list[0].Id != user.Id)
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("This nickname already used");
                        return;
                    }                
                }
            }

                    user.Nikname = NiknameBoxText.Text;
            saveNikname.Visibility = Visibility.Hidden;

            DoSave(NiknameBox, NiknameBoxText, ChangeNikname);
        }
        private void ChangeRealName_Click(object sender, RoutedEventArgs e)
        {
            ChangeRealName.Visibility = Visibility.Hidden;

            DoChange(RealNameBox, RealNameText, saveRealName);
        }
        private void SaveRealName_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (RealNameText.Text.Replace(" ", "").Length <= 0)
                    return;
                if (RealNameText.Text.Replace(" ", "").Length < 3)
                {
                    throw new Exception("Name is too small");
                }
                if (RealNameText.Text.Replace(" ", "").Length > 60)
                {
                    throw new Exception("Name is too long");
                }
                foreach (var sim in RealNameText.Text)
                {
                    if (int.TryParse(Convert.ToString(sim), out int numb))
                    {
                        throw new Exception("The name cannot contain numbers");
                    }
                }

                user.RealName = RealNameText.Text;
                saveRealName.Visibility = Visibility.Hidden;

                DoSave(RealNameBox, RealNameText, ChangeRealName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ChangeBirthday_Click(object sender, RoutedEventArgs e)
        {
            YourYear.Visibility = Visibility.Visible;
            YourMonth.Visibility = Visibility.Visible;
            YourDay.Visibility = Visibility.Visible;
            DeletBirth.Visibility = Visibility.Visible;

            ChangeBirtgday.Visibility = Visibility.Hidden;
            List<string> birth = new List<string>();

            if (user.Birthday != null)
                birth = (BirthdayBox.Content.ToString() ?? "").Split("-").ToList();
            else
            {
                birth.Add(DateTime.Now.Year.ToString());
                birth.Add(DateTime.Now.Month.ToString());
                birth.Add(DateTime.Now.Day.ToString());
            }

            YourYear.SelectedIndex = DateTime.Now.Year - Convert.ToInt32(birth[0]);
            YourMonth.SelectedIndex = Convert.ToInt32(birth[1]) - 1;
            YourDay.SelectedIndex = Convert.ToInt32(birth[2]) - 1;

            DoChange(BirthdayBox, null!, saveBirthday);
        }
        private void SaveBirthday_Click(object sender, RoutedEventArgs e)
        {
            YourYear.Visibility = Visibility.Hidden;
            YourMonth.Visibility = Visibility.Hidden;
            YourDay.Visibility = Visibility.Hidden;
            DeletBirth.Visibility = Visibility.Hidden;

            saveBirthday.Visibility = Visibility.Hidden;

            user.Birthday = YourYear.SelectedValue.ToString() + "-" +
                ((YourMonth.SelectedIndex + 1).ToString().Length == 1 ? "0" : "") +
                (YourMonth.SelectedIndex + 1) + "-" +
                ((YourDay.SelectedValue.ToString() ?? "").Length == 1 ? "0" : "") +
                YourDay.SelectedValue.ToString();

            BirthdayBox.Content = user.Birthday;

            DoSave(BirthdayBox, null!, ChangeBirtgday);
        }
        private void DeletBirth_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Delete you Birthday?", "hello", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            YourYear.Visibility = Visibility.Hidden;
            YourMonth.Visibility = Visibility.Hidden;
            YourDay.Visibility = Visibility.Hidden;
            DeletBirth.Visibility = Visibility.Hidden;

            saveBirthday.Visibility = Visibility.Hidden;

            user.Birthday = null;

            BirthdayBox.Content = System.Windows.Application.Current.Resources["EmptyBox"] as string;

            DoSave(BirthdayBox, null!, ChangeBirtgday);
        }
        private void ChangeGender_Click(object sender, RoutedEventArgs e)
        {
            ChangeGender.Visibility = Visibility.Hidden;

            ManText.Visibility = Visibility.Visible;
            WomanText.Visibility = Visibility.Visible;
            NoGenderText.Visibility = Visibility.Visible;

            radioMan.Visibility = Visibility.Visible;
            radioWoman.Visibility = Visibility.Visible;
            radioNoGender.Visibility = Visibility.Visible;

            DoChange(GenderBox, null!, SaveGender);
        }
        private void SaveGender_Click(object sender, RoutedEventArgs e)
        {
            SaveGender.Visibility = Visibility.Hidden;

            ManText.Visibility = Visibility.Hidden;
            WomanText.Visibility = Visibility.Hidden;
            NoGenderText.Visibility = Visibility.Hidden;

            radioMan.Visibility = Visibility.Hidden;
            radioWoman.Visibility = Visibility.Hidden;
            radioNoGender.Visibility = Visibility.Hidden;

            if (radioMan.IsChecked == true)
            {
                user.Gender = "m";
                GenderBox.Content = System.Windows.Application.Current.Resources["Man"] as string;
            }
            else if (radioWoman.IsChecked == true)
            {
                user.Gender = "w";
                GenderBox.Content = System.Windows.Application.Current.Resources["Woman"] as string;
            }
            else
            {
                user.Gender = null;
                GenderBox.Content = System.Windows.Application.Current.Resources["EmptyBox"] as string;
            }

            DoSave(GenderBox, null!, ChangeGender);
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            if (textImg.Source != null &&
                MessageBox.Show("You know, you not finish change Icon", "hello", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }
            if (NiknameBoxText.Visibility == Visibility.Visible &&
                MessageBox.Show("You know, you not finish change Nickname", "hello", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }
            if (RealNameText.Visibility == Visibility.Visible &&
                MessageBox.Show("You know, you not finish change Real Name", "hello", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }
            if (YourYear.Visibility == Visibility.Visible &&
                MessageBox.Show("You know, you not finish change Birthday", "hello", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }
            if (change)
            {
                using (var context = new SuperCatContext())
                {
                    if (admin)
                    {
                        NavigationService.Navigate(new AllFriends(context.UsersInfo.Where(x => x.Id == 1).First(), true));

                        return;
                    }
                    NavigationService.Navigate(new MyList(context.UsersInfo.First(x => x.Id == user.Id)));
                }

                return;
            }

            NavigationService.GoBack();
        }

        private void DeleteAccount_Click(object sender, MouseButtonEventArgs e)
        {
            
            if (MessageBox.Show("Delete this account?", "hello", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            using (var contecst = new SuperCatContext())
            {
                var arr = contecst.UsersInfo.Where(u => u.Id == user.Id).First();

                contecst.UsersInfo.Remove(arr);

                contecst.SaveChanges();
            }
            using (var contecst = new SuperCatContext())
            {
                foreach (var chi in contecst.Friends.ToList())
                {
                    if(chi.FfriendId == user.Id || chi.SfriendId == user.Id)
                    contecst.Friends.Remove(chi);
                }

                contecst.SaveChanges();
            }

            using (var contextGroup = new SuperCatContext())
            using (var contextUser = new SuperCatContext())
            {
                List<GroupInfo> group = contextGroup.GroupsInfo.Where(x => x.OwnerId == user.Id).ToList();
                List<int> delInclude = new List<int>();
                List<int> delJoin = new List<int>();

                foreach (var chi in group)
                {
                    delInclude = HelpWork.UnboxStringByList(chi.GroupMembers, user.Id);
                    delJoin = HelpWork.UnboxStringByList(chi.GroupJoin);


                    foreach (var userId in delInclude)
                    {
                        string result = HelpWork.UnboxStringByString(
                            contextUser.UsersInfo.Where(x => x.Id == userId).First().GroupsInclude, chi.Id);

                        contextUser.UsersInfo.Where(x => x.Id == userId).First().GroupsInclude = result;
                    }
                    foreach (var userId in delJoin)
                    {
                        string result = HelpWork.UnboxStringByString(
                            contextUser.UsersInfo.Where(x => x.Id == userId).First().GroupsThink, chi.Id);

                        contextUser.UsersInfo.Where(x => x.Id == userId).First().GroupsThink = result;
                    }

                    contextGroup.GroupsInfo.Remove(chi);

                    contextUser.SaveChanges();
                    contextGroup.SaveChanges();
                }
            }
            using (var context = new SuperCatContext())
            using (var contextGroup = new SuperCatContext())
            {
                List<int> delIncl = new List<int>();
                List<int> delJoin = new List<int>();

                foreach (var chi in user.GroupsInclude.Split("."))
                {
                    if (int.TryParse(chi, out int i))
                    {
                        if (contextGroup.GroupsInfo.Where(x => x.Id == i).FirstOrDefault() != null)
                        {
                            delIncl.Add(i);
                        }
                    }
                }
                delJoin = HelpWork.UnboxStringByList(user.GroupsThink);

                foreach (var GroupWhereMember in delIncl)
                {
                    contextGroup.GroupsInfo.Where(x => x.Id == GroupWhereMember).First().GroupMembers =
                       HelpWork.UnboxStringByString(
                        contextGroup.GroupsInfo.Where(x => x.Id == GroupWhereMember).First().GroupMembers,
                        user.Id);
                }
                foreach (var GroupWhereJoin in delJoin)
                {
                    contextGroup.GroupsInfo.Where(x => x.Id == GroupWhereJoin).First().GroupJoin =
                       HelpWork.UnboxStringByString(
                        contextGroup.GroupsInfo.Where(x => x.Id == GroupWhereJoin).First().GroupJoin,
                        user.Id);
                }

                context.SaveChanges();
                contextGroup.SaveChanges();
            }

                NavigationService.Navigate(new LogPage());
        }
        private void GoOut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var res = MessageBox.Show("Go out?", "hello", MessageBoxButton.YesNo);
            //var res = MessageBox.Show(System.Windows.Application.Current.Resources["TestMessage"] as string, "hello", MessageBoxButton.YesNo);
            //пример вывода сообщения из ресурсов(пока что не удалять)

            if (res != MessageBoxResult.Yes)
            {
                return;
            }

            NavigationService.Navigate(new LogPage());
        }

        private void FindNewImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.EndInit();

                textImg.Source = bitmap;
                ButtonNewImage.Visibility = Visibility.Hidden;
                buttonImageChange.IsEnabled = true;
            }
        }

        private void NowImage_Loaded(object sender, RoutedEventArgs e)
        {
            NowImage.Source = HelpWork.LoadImageFromByte(user.Image ?? new byte[0]);
        }

        private void buttonImageChange_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Change Image", "hello", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            change = true;
            buttonImageChange.IsEnabled = false;
            ButtonNewImage.Visibility = Visibility.Visible;

            BitmapSource bitmapSource = (BitmapSource)textImg.Source;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            byte[] imageBytes = new byte[0];

            using (MemoryStream memoryStream = new MemoryStream())
            {
                encoder.Save(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            NowImage.Source = HelpWork.LoadImageFromByte(imageBytes);
            textImg.Source = null;

            using (var context = new SuperCatContext())
            {
                context.UsersInfo.Where(u => u.Id == user.Id).First().Image = imageBytes;

                context.SaveChanges();
            }
        }

        private void GetYear()
        {
            for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 150; i--)
            {
                YourYear.Items.Add(i);
            }

            YourYear.SelectedIndex = 0;
        }
        private void GetMonth(bool all = false)
        {
            YourMonth.Items.Clear();

            var MonthName = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames;

            int step = (all) ? 12 : DateTime.Now.Month;

            for (int i = 0; i < step; i++)
            {
                YourMonth.Items.Add(MonthName[i]);
            }

            YourMonth.SelectedIndex = 0;
        }
        private void GetDay(bool all = false)
        {
            if (YourMonth.SelectedIndex == -1)
                return;

            YourDay.Items.Clear();

            int step = (all) ? DateTime.DaysInMonth(Convert.ToInt32(YourYear.SelectedValue.ToString()), Convert.ToInt32(YourMonth.SelectedIndex + 1)) : DateTime.Now.Day;

            for (int i = 0; i < step; i++)
            {
                YourDay.Items.Add(i + 1);
            }

            YourDay.SelectedIndex = 0;
        }
        private void SelectionChangedYear(object sender, SelectionChangedEventArgs e)
        {
            if (YourYear.SelectedValue.ToString() == DateTime.Now.Year.ToString())
            {
                GetMonth();

                return;
            }

            GetMonth(true);
        }
        private void SelectionChangedMonth(object sender, SelectionChangedEventArgs e)
        {
            if (YourMonth.SelectedIndex == -1)
                return;

            if (YourMonth.SelectedIndex + 1 == DateTime.Now.Month && YourYear.SelectedIndex == 0)
            {
                GetDay();

                return;
            }

            GetDay(true);
        }

        private void DefoultImg_Click(object sender, RoutedEventArgs e)
        {
            indexImg = (indexImg >= 3) ? 0 : indexImg + 1;

            textImg.Source = HelpWork.LoadImageFromByte(defImg.img[indexImg]);   
            ButtonNewImage.Visibility = Visibility.Hidden;
            buttonImageChange.IsEnabled = true;
        }

        private void ChangeEmail_Click(object sender, RoutedEventArgs e)
        {
            ChangeEmail.Visibility = Visibility.Hidden;

            if (emailRegex.IsMatch(EmailBoxText.Text))
            {
                DoChange(EmailBox, EmailBoxText, SaveEmail);
            }
            else
            {
                EmailBoxText.Visibility = Visibility.Visible;
                EmailBoxText.Text = string.Empty;
                DoChange(EmailBox, null!, SaveEmail);
            }

            EmailBoxText.Focus();
        }
        private void SaveEmail_Click(object sender, RoutedEventArgs e)
        {
            if(EmailBoxText.Text.Replace(" ", "").Length <= 0)
            {
                user.Email = null;
                EmailBox.Content = System.Windows.Application.Current.Resources["EmptyBox"] as string;
            }
            else
            {
                if (!emailRegex.IsMatch(EmailBoxText.Text))
                {
                    MessageBox.Show(System.Windows.Application.Current.Resources["EmailError"] as string);
                    return;
                }

                user.Email = EmailBoxText.Text;
                EmailBox.Content = EmailBoxText.Text;
            }

            SaveEmail.Visibility = Visibility.Hidden;

            EmailBoxText.Visibility = Visibility.Hidden;

            DoSave(EmailBox, null!, ChangeEmail);
        }

        private void oldPasswordLable_GotFocusTop(object sender, RoutedEventArgs e)
        {
            OldPasswordTextBox.Focus();
        }
        private void newPasswordLable_GotFocusTop(object sender, RoutedEventArgs e)
        {
            NewPasswordTextBox.Focus();
        }
        private void oldPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            oldPasswordLable.Visibility = Visibility.Hidden;
        }
        private void newPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            newPasswordLable.Visibility = Visibility.Hidden;
        }
        private void OldPasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(OldPasswordTextBox.Password.Length <= 0)
            {
                oldPasswordLable.Visibility = Visibility.Visible;
            }
        }
        private void NewPasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NewPasswordTextBox.Password.Length <= 0)
            {
                newPasswordLable.Visibility = Visibility.Visible;
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!HelpWork.VerifyPassword(OldPasswordTextBox.Password, user.Password))
                {
                    throw new Exception("Uncorrect old password");
                }

                bool charBool = false;
                bool numbBool = false;

                if (NewPasswordTextBox.Password.Length <= 0)
                {
                    throw new Exception("New Password is empty");
                }

                if (NewPasswordTextBox.Password.Length >= 4 && NewPasswordTextBox.Password.Length <= 15)
                {
                    foreach (char c in NewPasswordTextBox.Password)
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
                        MessageBox.Show("All Correct");

                        user.Password = HelpWork.HashPassword(NewPasswordTextBox.Password);

                        NewPasswordTextBox.Password = string.Empty;
                        OldPasswordTextBox.Password = string.Empty;
                        oldPasswordLable.Visibility = Visibility.Visible;
                        newPasswordLable.Visibility = Visibility.Visible;

                        work.userRepository.Update(user);
                        work.Save();

                        return;
                    }
                }
                throw new Exception("Unaize new password");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Uncorrect Password");
            }
        }
        private void ShowPass(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            liyOldPassword.Text = OldPasswordTextBox.Password;
            liyNewPassword.Text = NewPasswordTextBox.Password;

            liyOldPassword.Visibility = Visibility.Visible;
            liyNewPassword.Visibility = Visibility.Visible;

            OldPasswordTextBox.Visibility = Visibility.Collapsed;
            NewPasswordTextBox.Visibility = Visibility.Collapsed;
        }

        private void UnshowPass(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            liyOldPassword.Visibility = Visibility.Hidden;
            liyNewPassword.Visibility = Visibility.Hidden;

            OldPasswordTextBox.Visibility = Visibility.Visible;
            NewPasswordTextBox.Visibility = Visibility.Visible;
        }
    }
}

using SuperCat.Context;
using SuperCat.MyObjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SuperCat.GlobalFanc;
using System.Drawing;
using MaterialDesignColors.Recommended;
using System.Windows.Media;

namespace SuperCat.Pages.Group
{
    public partial class CreateNewGroup : Page
    {
        private UserInfo user = null!;
        private GroupInfo group = null!;

        public CreateNewGroup()
        {
            InitializeComponent();
        }
        public CreateNewGroup(UserInfo user, GroupInfo group = null!):this()
        {
            this.user = user;
            this.group = group;

            if(group == null)
            {
                CreateGroup.Visibility = Visibility.Visible;
            }
            else
            {
                DeleteButon.Visibility = Visibility.Visible;
                InviteButton.Visibility = Visibility.Visible;
                UserList.Visibility = Visibility.Visible;
                UpdateGroup.Visibility = Visibility.Visible;

                UpdateGroup.IsEnabled = true;

                FillPage();
            }
        }

        private void FillPage()
        {
            textImg.Source = HelpWork.LoadImageFromByte(group.Icon!);
            NameText.Text = group.Name;
            Place.SelectedIndex = (group.MaxPeople - 4);

            using (var context = new SuperCatContext())
            {
                foreach (var id in HelpWork.UnboxStringByList(group.GroupJoin))
                {
                    UserInfo userN = context.UsersInfo.First(x => x.Id == id);

                    ComboBoxItem item = new ComboBoxItem
                    {
                        FontSize = 18,
                        Padding = new Thickness(50, 0, 0, 0),
                        Content = $"({userN.Id.ToString("D10")}) {userN.Nikname}",
                        Background = System.Windows.Media.Brushes.Orange,
                    };

                    UserList.Items.Add(item);
                }
                foreach (var id in HelpWork.UnboxStringByList(group.GroupMembers))
                {
                    if(id == 1 || id == user.Id)
                    {
                        continue;
                    }

                    UserInfo userN = context.UsersInfo.First(x => x.Id == id);

                    ComboBoxItem item = new ComboBoxItem
                    {
                        FontSize = 18,
                        Padding = new Thickness(50, 0, 0, 0),
                        Content = $"({userN.Id.ToString("D10")}) {userN.Nikname}",
                        Background = System.Windows.Media.Brushes.LightGreen,
                    };

                    UserList.Items.Add(item);
                }
            }
        }
        public void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void FindNewImage(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.EndInit();

                textImg.Source = bitmap;
            }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox combo = (sender as System.Windows.Controls.ComboBox)!;

            for (int i = 3; i <= 15; i++)
            {
                combo.Items.Add(new ComboBoxItem
                {
                    Content = i,
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center
                });
            }

            combo.SelectedIndex = (group == null)?0:(group.MaxPeople-4);
        }


        private void NameText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key != Key.Enter)
            {
                return;
            }

            Place.Focus();
        }

        private void NameText_LostFocus(object sender, RoutedEventArgs e)
        {
            if(NameText.Text.Length < 4)
            {
                System.Windows.MessageBox.Show("This Name is too small");
                CreateGroup.IsEnabled = false;
                return;
            }
            if(NameText.Text.Length > 15)
            {
                System.Windows.MessageBox.Show("This Name is too Long");
                CreateGroup.IsEnabled = false;
                return;
            }

            using (var context = new SuperCatContext())
            {
                foreach (var item in context.GroupsInfo.Where(x=>x.OwnerId == user.Id))
                {
                    if(item.Name.Equals(NameText.Text))
                    {
                        if(group != null && group.Name.Equals(NameText.Text))
                        {
                            continue;
                        }
                        System.Windows.MessageBox.Show("You already have a group with this name");
                        CreateGroup.IsEnabled = false;
                        return;
                    }
                }
            }

            CreateGroup.IsEnabled = true;
        }
        private void createGroup_Click(object sender, RoutedEventArgs e)
        {
            GroupInfo newG = new GroupInfo(user.Id, NameText.Text,
                (Place.SelectedIndex + 4), HelpWork.GetBytesImageSource(textImg.Source));

            using (var context = new SuperCatContext())
            using (var contextUser = new SuperCatContext())
            {
                context.GroupsInfo.Add(newG);

                context.SaveChanges();

                List<GroupInfo> thisName = context.GroupsInfo.Where(x=>x.OwnerId == user.Id).ToList();
                int id = thisName.Where(x=>x.Name.Equals(newG.Name)).First().Id;

                contextUser.UsersInfo.Where(x => x.Id == 1).First().GroupsInclude += ("." + id);
                contextUser.UsersInfo.First(x => x.Id == user.Id).GroupsInclude += ("." + id);
               
                contextUser.SaveChanges();
            }

            NavigationService.GoBack();
        }
        private void updateGroup_Click(object sender, RoutedEventArgs e)
        {
            if (NameText.Text.Length < 4)
            {
                System.Windows.MessageBox.Show("This Name is too small");
                return;
            }
            if (NameText.Text.Length > 15)
            {
                System.Windows.MessageBox.Show("This Name is too Long");
                return;
            }

            using (var context = new SuperCatContext())
            {
                foreach (var item in context.GroupsInfo.Where(x => x.OwnerId == user.Id))
                {
                    if (item.Name.Equals(NameText.Text))
                    {
                        if (group != null && group.Name.Equals(NameText.Text))
                        {
                            continue;
                        }
                        System.Windows.MessageBox.Show("You already have a group with this name");
                        return;
                    }
                }
            }

            using (var context = new SuperCatContext())
            {
                int memberSize = HelpWork.UnboxStringByList(context.GroupsInfo.First(x => x.Id == group.Id).GroupMembers).Count();
                int joinSize = HelpWork.UnboxStringByList(context.GroupsInfo.First(x => x.Id == group.Id).GroupJoin).Count();
                int max = context.GroupsInfo.First(x => x.Id == group.Id).MaxPeople;

                if ((memberSize + joinSize) > Place.SelectedIndex+4)
                {
                    System.Windows.MessageBox.Show("This group is full");
                    return;
                }


                context.GroupsInfo.First(x => x.Id == group.Id).Icon = HelpWork.GetBytesImageSource(textImg.Source);
                context.GroupsInfo.First(x => x.Id == group.Id).Name = NameText.Text.ToString();
                context.GroupsInfo.First(x => x.Id == group.Id).MaxPeople = Place.SelectedIndex + 4;

                context.SaveChanges();
            }

            NavigationService.GoBack();
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserList.SelectedIndex == 0)
            {
                DeleteButon.IsEnabled = false;
                InviteButton.IsEnabled = false;
                UserList.Background = null;

                return;
            }

            ComboBoxItem elem = (UserList.SelectedItem as ComboBoxItem)!;

            if (((SolidColorBrush)elem.Background).Color == System.Windows.Media.Brushes.LightGreen.Color)
            {
                InviteButton.IsEnabled = false;
            }
            else
            {
                InviteButton.IsEnabled = true;
            }
            DeleteButon.IsEnabled = true;
            UserList.Background = elem.Background;
        }

        private void DeleteButon_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem elem = (UserList.SelectedItem as ComboBoxItem)!;
            int id = Convert.ToInt32(new string((elem.Content.ToString()!.Split(")"))[0].Skip(1).ToArray()));


            using (var contextUser = new SuperCatContext())
            using (var contextGroup = new SuperCatContext())
            {
                UserInfo use = contextUser.UsersInfo.First(x => x.Id == id);

                if(System.Windows.MessageBox.Show($"You want to delete {use.Nikname}", "Delete", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    return;
                }

                if (elem.Background.Equals(System.Windows.Media.Brushes.LightGreen))
                {
                    contextGroup.GroupsInfo.First(x => x.Id == group.Id).GroupMembers = HelpWork.UnboxStringByString(
                        contextGroup.GroupsInfo.First(x => x.Id == group.Id).GroupMembers, id);

                    contextUser.UsersInfo.First(x => x.Id == id).GroupsInclude = HelpWork.UnboxStringByString(
                        use.GroupsInclude, group.Id);
                }
                else
                {
                    contextGroup.GroupsInfo.First(x => x.Id == group.Id).GroupJoin = HelpWork.UnboxStringByString(
                        contextGroup.GroupsInfo.First(x => x.Id == group.Id).GroupJoin, id);

                    contextUser.UsersInfo.First(x => x.Id == id).GroupsThink = HelpWork.UnboxStringByString(
                        use.GroupsThink, group.Id);
                }

                contextGroup.SaveChanges();
                contextUser.SaveChanges();
            }

            UserList.SelectedIndex = 0;
            UserList.Items.Remove(elem);
        }

        private void InviteButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem elem = (UserList.SelectedItem as ComboBoxItem)!;
            int id = Convert.ToInt32(new string((elem.Content.ToString()!.Split(")"))[0].Skip(1).ToArray()));


            using (var contextUser = new SuperCatContext())
            using (var contextGroup = new SuperCatContext())
            {
                UserInfo use = contextUser.UsersInfo.First(x => x.Id == id);

                if (System.Windows.MessageBox.Show($"You want to include {use.Nikname}", "include", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    return;
                }

                contextGroup.GroupsInfo.First(x => x.Id == group.Id).GroupJoin = HelpWork.UnboxStringByString(
                    contextGroup.GroupsInfo.First(x => x.Id == group.Id).GroupJoin, id);

                contextUser.UsersInfo.First(x => x.Id == id).GroupsThink = HelpWork.UnboxStringByString(
                    use.GroupsThink, group.Id);

                List<int> memberUser = HelpWork.UnboxStringByList(use.GroupsInclude);
                List<int> memberGroup = HelpWork.UnboxStringByList(contextGroup.GroupsInfo.First(x => x.Id == group.Id).GroupMembers);

                memberGroup.Add(id);
                memberUser.Add(group.Id);

                contextGroup.GroupsInfo.First(x => x.Id == group.Id).GroupMembers = HelpWork.UnboxListByString(memberGroup);

                contextUser.UsersInfo.First(x => x.Id == id).GroupsInclude = HelpWork.UnboxListByString(memberUser);

                contextGroup.SaveChanges();
                contextUser.SaveChanges();
            }

            UserList.SelectedIndex = 0;
            elem.Background = System.Windows.Media.Brushes.LightGreen;
        }
    }
}
using SuperCat.MyObjects;
using SuperCat.Context;
using System.Windows;
using System.Windows.Controls;
using SuperCat.Pages.ChatsList;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Windows.Input;
using SuperCat.GlobalFanc;
using Microsoft.EntityFrameworkCore;

namespace SuperCat.Pages.Group
{
    /// <summary>
    /// Interaction logic for AllMyGroups.xaml
    /// </summary>
    public partial class AllMyGroups : Page
    {
        private UserInfo user = null!;
        private List<GroupInfo> showGroups = null!;

        public AllMyGroups()
        {
            InitializeComponent();
        }
        public AllMyGroups(UserInfo user) : this()
        {
            this.user = user;
            showGroups = new List<GroupInfo>();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUser();
            if (LocateBy.SelectedIndex == 0)
            {
                LocateBy_SelectionChanged(sender, null!);
                return;
            }

            LocateBy.SelectedIndex = 0;
        }

        private Grid GetStartGroup(GroupInfo group)
        {
            Image icon = new Image
            {
                Stretch = defouldIconImage.Stretch,
                Source = HelpWork.LoadImageFromByte(group.Icon!)
            };
            Border iconBord = new Border
            {
                Height = defouldIconBord.Height,
                Width = defouldIconBord.Width,
                Margin = defouldIconBord.Margin,
                BorderThickness = defouldIconBord.BorderThickness,
                BorderBrush = defouldIconBord.BorderBrush
            };
            iconBord.Child = icon;

            TextBlock groupName = new TextBlock
            {
                VerticalAlignment = defouldGroupNameBlock.VerticalAlignment,
                HorizontalAlignment = defouldGroupNameBlock.HorizontalAlignment,
                Style = defouldGroupNameBlock.Style,
                Text = group.Name,
                TextTrimming = TextTrimming.CharacterEllipsis,
                FontSize = defouldGroupNameBlock.FontSize,
                Margin = defouldGroupNameBlock.Margin,
                Width = defouldGroupNameBlock.Width,
            };
            TextBlock owneName = new TextBlock
            {
                VerticalAlignment = defouldOwnerBlock.VerticalAlignment,
                HorizontalAlignment = defouldOwnerBlock.HorizontalAlignment,
                Style = defouldOwnerBlock.Style,
                TextTrimming = TextTrimming.CharacterEllipsis,
                FontSize = defouldOwnerBlock.FontSize,
                Margin = defouldOwnerBlock.Margin,
                Width = defouldOwnerBlock.Width,
            };
            using (var context = new SuperCatContext())
            {
                owneName.Text = context.UsersInfo.Where(x => x.Id == group.OwnerId).First().Nikname;
            }
            Label ready = new Label
            {
                VerticalAlignment = defouldSizeBlock.VerticalAlignment,
                HorizontalAlignment = defouldSizeBlock.HorizontalAlignment,
                Style = defouldSizeBlock.Style,
                Content = string.Empty,
                FontSize = defouldSizeBlock.FontSize,
                Margin = defouldSizeBlock.Margin,
            };

            if (group.OwnerId == user.Id || user.Id == 1)
            {
                ready.Content = System.Windows.Application.Current.Resources["YouOwnerText"] as string;
            }
            else
            {
                foreach (var item in group.GroupMembers.Split("."))
                {
                    if (int.TryParse(item, out int id) && id == user.Id)
                    {
                        ready.Content = System.Windows.Application.Current.Resources["YouMemberText"] as string;
                        break;
                    }
                }
                if (ready.Content!.ToString() == string.Empty)
                {
                    ready.Content = System.Windows.Application.Current.Resources["YouNotMemberText"] as string;
                }
            }

            Label Id = new Label
            {
                VerticalAlignment = defouldIdLabel.VerticalAlignment,
                HorizontalAlignment = defouldIdLabel.HorizontalAlignment,
                Style = defouldIdLabel.Style,
                Content = group.Id.ToString("D10"),
                FontSize = defouldIdLabel.FontSize,
                Margin = defouldIdLabel.Margin,
            };

            Grid _case = new Grid();

            _case.Children.Add(iconBord);
            _case.Children.Add(groupName);
            _case.Children.Add(owneName);
            _case.Children.Add(ready);
            _case.Children.Add(Id);

            return _case;
        }
        private Grid GetActive(Grid _case, Image img)
        {
            Image activ = new Image
            {
                Stretch = defouldTrashImage.Stretch,
                Cursor = Cursors.Hand,
                Source = img.Source
            };
            Border activeBord = new Border
            {
                Height = defouldTrashBord.Height,
                Cursor = Cursors.Hand,
                Width = defouldTrashBord.Width,
                Style = defouldTrashBord.Style,
                Padding = defouldTrashBord.Padding,
                Margin = defouldTrashBord.Margin,
                BorderThickness = defouldTrashBord.BorderThickness,
                BorderBrush = defouldTrashBord.BorderBrush,
            };
            activeBord.Child = activ;

            _case.Children.Add(activeBord);

            return _case;
        }
        private void BoxedElem(Grid _case)
        {
            Border mainCase = new Border
            {
                BorderBrush = defouldFoneBord.BorderBrush,
                BorderThickness = defouldFoneBord.BorderThickness,
                Height = defouldFoneBord.Height,
                CornerRadius = defouldFoneBord.CornerRadius,
                Margin = defouldFoneBord.Margin,
                Child = _case
            };

            mainCase.MouseEnter += defouldFoneBord_MouseEnter;
            mainCase.MouseLeave += defouldFoneBord_MouseLeave;
            mainCase.MouseDown += GoIntoGroupMessage;

            MyGroupArray.Children.Add(mainCase);
        }

        private void FillGroups()
        {
            MyGroupArray.Children.Clear();

            if (showGroups.Count <= 0)
            {
                EmptyText.Visibility = Visibility.Visible;
                return;
            }
            EmptyText.Visibility = Visibility.Collapsed;

            using (var context = new SuperCatContext())
            {
                foreach (var group in showGroups)
                {
                    List<int> memderId = HelpWork.UnboxStringByList(group.GroupMembers);
                    List<int> joinsId = HelpWork.UnboxStringByList(group.GroupJoin);
                    Grid _case = GetStartGroup(group);

                    if (YourGroups.Visibility == Visibility.Visible)
                    {
                        _case = GetActive(_case, defouldJoinImage);
                        _case.Children[5].MouseDown += defouldJoinBord_MouseDown;

                        BoxedElem(_case);
                        continue;
                    }

                    if (user.Id == 1 || user.Id == group.OwnerId)
                    {
                        _case = GetActive(_case, defouldTrashImage);
                        _case.Children[5].MouseDown += defouldTrashBord_Click;
                    }
                    else
                    {
                        _case = GetActive(_case, defouldGoOutImage);
                        _case.Children[5].MouseDown += defouldGoOutBord_Click;
                    }

                    BoxedElem(_case);
                }
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void defouldJoinBord_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            string friendNick = GetGroupName((sender as Border)!);

            if (MessageBox.Show(("You want to write \"" + friendNick + "\" in your group list"), "Delete", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            int id = GetGroupId((sender as Border)!);

            foreach (var item in user.GroupsThink.Split("."))
            {
                if (int.TryParse(item, out int i) && i == id)
                {
                    MessageBox.Show("You already have this group in your group list");
                    return;
                }
            }
            foreach (var item in user.GroupsInclude.Split("."))
            {
                if (int.TryParse(item, out int i) && i == id)
                {
                    MessageBox.Show("You already have this group in your group list");
                    return;
                }
            }

            using (var context = new SuperCatContext())
            using (var contextUser = new SuperCatContext())
            {
                int memberSize = HelpWork.UnboxStringByList(context.GroupsInfo.First(x => x.Id == id).GroupMembers).Count();
                int joinSize = HelpWork.UnboxStringByList(context.GroupsInfo.First(x => x.Id == id).GroupJoin).Count();
                int max = context.GroupsInfo.First(x => x.Id == id).MaxPeople;

                if ((memberSize + joinSize) >= max)
                {
                    MessageBox.Show("This group is full");
                    return;
                }

                context.GroupsInfo.First(x => x.Id == id).GroupJoin += "." + user.Id;
                context.SaveChanges();

                contextUser.UsersInfo.First(x => x.Id == user.Id).GroupsThink += "." + id;
                contextUser.SaveChanges();

                UpdateUser();
            }
        }
        private void defouldFoneBord_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Border)!.Background = Brushes.Orange;
        }
        private void defouldFoneBord_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Border)!.Background = null;
        }
        private int GetGroupId(Border g)
        {
            return Convert.ToInt32(((g.Parent as Grid)!.Children[4] as Label)!.Content);
        }
        private string GetGroupName(Border g)
        {
            return ((g.Parent as Grid)!.Children[1] as TextBlock)!.Text.ToString() ?? "";
        }

        private void defouldTrashBord_Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (MessageBox.Show(("you want to delete group " + GetGroupName((sender as Border)!)), "delete", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            List<int> usersInc = new List<int>();
            List<int> usersThink = new List<int>();
            int id = GetGroupId((sender as Border)!);

            using (var context = new SuperCatContext())
            {
                GroupInfo group = context.GroupsInfo.Where(x => x.Id == id).First();

                usersInc = HelpWork.UnboxStringByList(group.GroupMembers);
                usersThink = HelpWork.UnboxStringByList(group.GroupJoin);

                context.Remove(group);
                context.SaveChanges();
            }

            foreach (var i in usersInc)
            {
                GoOutGroup(i, id, true);
            }
            foreach (var i in usersThink)
            {
                GoOutGroup(i, id, false);
            }

            using (var context = new SuperCatContext())
            {
                this.user = context.UsersInfo.First(x => x.Id == user.Id);
            }

            LocateBy_SelectionChanged(sender, null!);
        }
        private void GoOutGroup(int userId, int groupId, bool include)
        {
            string newGroup = "";
            List<string> users = new List<string>();

            using (var context = new SuperCatContext())
            {
                if (include)
                {
                    users = context.UsersInfo.Where(x => x.Id == userId).First().GroupsInclude!.Split(".").ToList();
                }
                else
                {
                    users = context.UsersInfo.Where(x => x.Id == userId).First().GroupsThink!.Split(".").ToList();
                }

                newGroup = HelpWork.RecreateListByString(users, groupId);

                if (newGroup == "")
                {
                    if (include)
                    {
                        context.UsersInfo.Where(x => x.Id == userId).First().GroupsInclude = "";
                    }
                    else
                    {
                        context.UsersInfo.Where(x => x.Id == userId).First().GroupsThink = "";
                    }
                }
                else
                {
                    if (include)
                    {
                        context.UsersInfo.Where(x => x.Id == userId).First().GroupsInclude = newGroup;
                    }
                    else
                    {
                        context.UsersInfo.Where(x => x.Id == userId).First().GroupsThink = newGroup;
                    }
                }

                context.SaveChanges();
            }
        }
        private void defouldGoOutBord_Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            string friendNick = (((sender as Border)!.Parent as Grid)!.Children[1] as TextBlock)!.Text.ToString() ?? "";

            if (MessageBox.Show(("You want to remove \"" + friendNick + "\" from your group list"), "Delete", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            int id = GetGroupId((sender as Border)!);
            string result = string.Empty;
            bool inc = false;

            using (var context = new SuperCatContext())
            using (var contextUser = new SuperCatContext())
            {
                foreach (var item in context.GroupsInfo.First(x => x.Id == id).GroupMembers.Split("."))
                {
                    if (int.TryParse(item, out int i) && i != user.Id)
                    {
                        result += "." + i;
                        continue;
                    }
                    if (i == user.Id)
                    {
                        inc = true;
                    }
                }
                if (inc)
                {
                    context.GroupsInfo.First(x => x.Id == id).GroupMembers = result;

                    result = HelpWork.UnboxStringByString(
                        contextUser.UsersInfo.First(x => x.Id == user.Id).GroupsInclude, id);

                    context.UsersInfo.First(x => x.Id == user.Id).GroupsInclude = result;
                }
                else
                {
                    result = HelpWork.UnboxStringByString(
                        context.GroupsInfo.Where(x => x.Id == id).First().GroupJoin, user.Id);

                    context.GroupsInfo.First(x => x.Id == id).GroupJoin = result;

                    result = HelpWork.UnboxStringByString(
                        context.UsersInfo.First(x => x.Id == user.Id).GroupsThink, id);

                    context.UsersInfo.First(x => x.Id == user.Id).GroupsThink = result;
                }

                context.SaveChanges();
                contextUser.SaveChanges();

                UpdateUser();
            }

            LocateBy_SelectionChanged(sender, null!);
        }

        private void LocateBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (showGroups == null)
            {
                return;
            }

            if (e != null)
            {
                LocateText.Text = string.Empty;
            }

            showGroups.Clear();

            if (YourGroups.Visibility == Visibility.Visible)
            {
                FillGroups();
                return;
            }

            if (LocateBy.SelectedIndex == 1)
            {
                using (var context = new SuperCatContext())
                {
                    showGroups.AddRange(context.GroupsInfo.Where(x => x.OwnerId == user.Id).ToList());
                }
            }
            if (user.GroupsInclude != "" && (LocateBy.SelectedIndex == 2 || LocateBy.SelectedIndex == 0))
            {
                using (var context = new SuperCatContext())
                {
                    foreach (var item in user.GroupsInclude.Split("."))
                    {
                        if (int.TryParse(item, out int gr))
                        {
                            showGroups.Add(context.GroupsInfo.Where(x => x.Id == gr).First());
                        }
                    }
                }
            }
            if (user.GroupsThink != "" && (LocateBy.SelectedIndex == 3 || LocateBy.SelectedIndex == 0))
            {
                using (var context = new SuperCatContext())
                {
                    foreach (var item in user.GroupsThink.Split("."))
                    {
                        if (int.TryParse(item, out int gr))
                        {
                            showGroups.Add(context.GroupsInfo.Where(x => x.Id == gr).First());
                        }
                    }
                }
            }

            FillGroups();
        }
        public void UpdateUser()
        {
            using (var context = new SuperCatContext())
            {
                user = context.UsersInfo.First(x => x.Id == user.Id);
            }
        }
        private void LocateText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            Keyboard.ClearFocus();

            LocateText_LostFocus(sender, null!);
        }
        private async void LocateText_LostFocus(object sender, RoutedEventArgs e)
        {
            LocateBy_SelectionChanged(sender, null!);

            if (LocateText.Text.Length == 0)
            {
                return;
            }

            if (YourGroups.Visibility == Visibility.Visible)
            {
                using (var context = new SuperCatContext())
                {
                    if (LocateAllBy.SelectedIndex == 0)
                    {
                        showGroups = context.GroupsInfo.Where(x => x.Name.Contains(LocateText.Text)).ToList();
                    }
                    else
                    {
                        showGroups = context.GroupsInfo.Where(x => x.Id == Convert.ToInt32(LocateText.Text)).ToList();
                    }
                }

                List<GroupInfo> del = showGroups.Where(x => x.OwnerId == user.Id).ToList();

                foreach (var item in del)
                {
                    showGroups.Remove(item);
                }
            }
            else
            {
                showGroups = showGroups.Where(x => x.Name.Contains(LocateText.Text)).ToList();
            }
            FillGroups();

            var originalColor = LocateText.Foreground;

            if (showGroups.Count <= 0)
            {
                LocateText.Foreground = Brushes.Red;
            }
            else
            {
                LocateText.Foreground = Brushes.DarkGreen;
            }

            await Task.Delay(500);

            LocateText.Foreground = originalColor;
        }

        private void createGroup_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateNewGroup(user));
        }

        private void joinGroup_Click(object sender, RoutedEventArgs e)
        {
            CreateGroup.Visibility = Visibility.Collapsed;
            joinGroup.Visibility = Visibility.Collapsed;
            LocateBy.Visibility = Visibility.Collapsed;

            YourGroups.Visibility = Visibility.Visible;
            LocateAllBy.Visibility = Visibility.Visible;

            LocateText.Text = string.Empty;

            LocateBy_SelectionChanged(sender, null!);
        }

        private void YourGroups_Click(object sender, RoutedEventArgs e)
        {
            YourGroups.Visibility = Visibility.Collapsed;
            LocateAllBy.Visibility = Visibility.Collapsed;

            CreateGroup.Visibility = Visibility.Visible;
            joinGroup.Visibility = Visibility.Visible;
            LocateBy.Visibility = Visibility.Visible;

            LocateText.Text = string.Empty;

            LocateBy_SelectionChanged(sender, null!);
        }

        private void GoIntoGroupMessage(object sender, MouseButtonEventArgs e)
        {
            int groupId = Convert.ToInt32((((sender as Border)!.Child as Grid)!.Children[4] as Label)!.Content);

            using (var context = new SuperCatContext())
            {
                GroupInfo gr = context.GroupsInfo.First(x => x.Id == groupId);
                int memberSize = HelpWork.UnboxStringByList(gr.GroupMembers).Count();
                int joinSize = HelpWork.UnboxStringByList(gr.GroupJoin).Count();

                if (!HelpWork.UnboxStringByList(gr.GroupMembers).Contains(user.Id))
                {
                    MessageBox.Show("You are not the member this group");
                    return;
                }

                NavigationService.Navigate(new GoGroupChat(user.Id, context.GroupsInfo.First(x=>x.Id == groupId)));
            }
        }
    }
}

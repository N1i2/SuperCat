using MaterialDesignThemes.Wpf;
using Microsoft.IdentityModel.Tokens;
using SuperCat.Context;
using SuperCat.GlobalFanc;
using SuperCat.MyObjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace SuperCat.Pages.FriendFile
{
    /// <summary>
    /// Interaction logic for AllFriends.xaml
    /// </summary>
    public partial class AllFriends : Page
    {
        private UserInfo user = null!;
        private List<UserInfo> userInfos = new List<UserInfo>();
        private List<MyObjects.Friend> myFriends = null!;
        private List<MyObjects.Friend> thinkFriend = null!;
        private List<MyObjects.Friend> Repuests = null!;

        public AllFriends()
        {
            InitializeComponent();
        }
        public AllFriends(UserInfo user):this()
        {
            this.user = user;
            myFriends = new List<MyObjects.Friend>();
            thinkFriend = new List<MyObjects.Friend>();
            Repuests = new List<MyObjects.Friend>();

            FillFriends();

            this.PreviewKeyDown += new KeyEventHandler(Page_PreviewKeyDown);

            MyFriendArray.Focus();
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var scrollViewer = this.MyFriendArray.Parent as ScrollViewer;

            if (scrollViewer != null)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        scrollViewer.LineUp();
                        break;
                    case Key.Down:
                        scrollViewer.LineDown();
                        break;
                }
            }
        }
        private void FillFriends()
        {
            int item = LocateBy.SelectedIndex;

            if (user == null)
                return;

            GetFriends();
        }

        private void AddReadyArea(Grid _case)
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
            mainCase.MouseDown += defouldFoneBord_MouseDown;

            MyFriendArray.Children.Add(mainCase);
        }

        private Grid GetCaseForFriend(UserInfo friend)
        {
            Image icon = new Image
            {
                Stretch = defouldIconImage.Stretch,
                Source = HelpWork.LoadImageFromByte(friend.Image!)
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

            Label nick = new Label
            {
                VerticalAlignment = defouldNicknameLabel.VerticalAlignment,
                HorizontalAlignment = defouldNicknameLabel.HorizontalAlignment,
                Style = defouldNicknameLabel.Style,
                Content = friend.Nikname,
                FontSize = defouldNicknameLabel.FontSize,
                Margin = defouldNicknameLabel.Margin,
                Width = defouldNicknameLabel.Width,
            };
            Label realN = new Label
            {
                VerticalAlignment = defouldRealNameLabel.VerticalAlignment,
                HorizontalAlignment = defouldRealNameLabel.HorizontalAlignment,
                Style = defouldRealNameLabel.Style,
                Content = friend.RealName,
                FontSize = defouldRealNameLabel.FontSize,
                Margin = defouldRealNameLabel.Margin,
                Width = defouldRealNameLabel.Width,
            };
            Label Id = new Label
            {
                VerticalAlignment = defouldIdLabel.VerticalAlignment,
                HorizontalAlignment = defouldIdLabel.HorizontalAlignment,
                Style = defouldIdLabel.Style,
                Content = friend.Id.ToString("D10"),
                FontSize = defouldIdLabel.FontSize,
                Margin = defouldIdLabel.Margin,
            };

            Image del = new Image
            {
                Stretch = defouldTrashImage.Stretch,
                Cursor = defouldMailBord.Cursor,
                Source = defouldTrashImage.Source
            };
            Border delBord = new Border
            {
                Height = defouldTrashBord.Height,
                Cursor = defouldMailBord.Cursor,
                Width = defouldTrashBord.Width,
                Style = defouldTrashBord.Style,
                Padding = defouldTrashBord.Padding,
                Margin = defouldTrashBord.Margin,
                BorderThickness = defouldTrashBord.BorderThickness,
                BorderBrush = defouldTrashBord.BorderBrush,
            };
            delBord.MouseDown += defouldTrashBord_Click;
            delBord.Child = del;

            Grid _case = new Grid();
            _case.Children.Add(iconBord);
            _case.Children.Add(nick);
            _case.Children.Add(realN);
            _case.Children.Add(Id);
            _case.Children.Add(delBord);

            return _case;
        }
        private Grid GetWantFriend(Grid _case)
        {
            Label GetReq = new Label
            {
                VerticalAlignment = defouldUnderCons.VerticalAlignment,
                HorizontalAlignment = defouldUnderCons.HorizontalAlignment,
                Content = Application.Current.Resources["ConiderationText"] as string,
                FontSize = defouldUnderCons.FontSize,
                Style = defouldUnderCons.Style,
                Margin = defouldUnderCons.Margin,
            };

            _case.Children.Add(GetReq);

            return _case;
        }
        private Grid GetThinkFriend(Grid _case)
        {
            Image yes = new Image
            {
                Stretch = defouldMailImage.Stretch,
                Cursor = defouldMailBord.Cursor,
                Source = defouldYesImage.Source
            };
            Border yesBord = new Border
            {
                Cursor = defouldMailBord.Cursor,
                Height = defouldMailBord.Height,
                Width = defouldMailBord.Width,
                Style = defouldMailBord.Style,
                Padding = defouldMailBord.Padding,
                Margin = defouldMailBord.Margin,
                BorderThickness = defouldMailBord.BorderThickness,
                BorderBrush = defouldMailBord.BorderBrush,
            };
            yesBord.Child = yes;
            yesBord.MouseDown += AgreeFriend_Click;


            _case.Children.Add(yesBord);

            return _case;
        }
        private Grid GetRealyFriend(Grid _case)
        {
            Image mail = new Image
            {
                Stretch = defouldMailImage.Stretch,
                Cursor = defouldMailBord.Cursor,
                Source = defouldMailImage.Source
            };
            Border mailBord = new Border
            {
                Cursor = defouldMailBord.Cursor,
                Height = defouldMailBord.Height,
                Width = defouldMailBord.Width,
                Style = defouldMailBord.Style,
                Padding = defouldMailBord.Padding,
                Margin = defouldMailBord.Margin,
                BorderThickness = defouldMailBord.BorderThickness,
                BorderBrush = defouldMailBord.BorderBrush,
            };
            mailBord.Child = mail;

            _case.Children.Add(mailBord);

            return _case;
        }
        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void defouldFoneBord_MouseEnter(object sender, MouseEventArgs e)
        {
           (sender as Border)!.Background = Brushes.Orange;
        }
        private void defouldFoneBord_MouseLeave(object sender, MouseEventArgs e)
        {
           (sender as Border)!.Background = null;
        }

        private void defouldTrashBord_Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            string friendNick = (((sender as Border)!.Parent as Grid)!.Children[1] as Label)!.Content.ToString()??"";

            if (MessageBox.Show(("You want to remove \"" + friendNick + "\" from your friends list"), "Delete", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            int userId = Convert.ToInt32((((sender as Border)!.Parent as Grid)!.Children[3] as Label)!.Content);
            MyObjects.Friend locF = null!;

            foreach (var friend in myFriends)
            {
                if((friend.FfriendId == user.Id && friend.SfriendId == userId) ||
                    ((friend.FfriendId == userId && friend.SfriendId == user.Id)))
                {
                    locF = friend;
                    break;
                }
            }
            if (locF == null)
            {
                foreach (var friend in Repuests)
                {
                    if (friend.FfriendId == user.Id && friend.WhoThink == userId)
                    {
                        locF = friend;
                        break;
                    }
                }
            }
            if (locF == null)
            {
                foreach (var friend in thinkFriend)
                {
                    if (friend.FfriendId == userId && friend.WhoThink == user.Id)
                    {
                        locF = friend;
                        break;
                    }
                }
            }

            using (var context = new SuperCatContext())
            {
                var friendToRemove = context.Friends.FirstOrDefault(f => f.Id == locF.Id);
                if (friendToRemove != null)
                {
                    context.Friends.Remove(friendToRemove);
                    context.SaveChanges();
                }
            }

            FillFriends();
        }
        private void AgreeFriend_Click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            string friendNick = (((sender as Border)!.Parent as Grid)!.Children[1] as Label)!.Content.ToString() ?? "";

            if (MessageBox.Show(("You want to write \"" + friendNick + "\" in your friends list"), "Delete", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            int userId = Convert.ToInt32((((sender as Border)!.Parent as Grid)!.Children[3] as Label)!.Content);
            MyObjects.Friend locF = new MyObjects.Friend();

            foreach (var friend in thinkFriend)
            {
                if (friend.FfriendId == userId &&
                    friend.WhoThink == user.Id)
                {
                    locF = friend;
                    break;
                }
            }

            using (var context = new SuperCatContext())
            {
                context.Friends.Where(x => x.FfriendId == locF.FfriendId).First().SfriendId = Convert.ToInt32(locF.WhoThink);
                context.Friends.Where(x => x.Id == locF.Id).First().WhoThink = null;

                context.SaveChanges();
            }

            FillFriends();
        }

        private void LocateBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillFriends();
        }

        private void LocateText_LostFocus(object sender, RoutedEventArgs e)
        {
            GetFriends();
        }

        private void GetFriends()
        {
            MyFriendArray.Children.Clear();
            EmptyText.Visibility = Visibility.Collapsed;
            myFriends.Clear();
            thinkFriend.Clear();
            Repuests.Clear();
            userInfos.Clear();

            using (var context = new SuperCatContext())
            using (var contextUser = new SuperCatContext())
            {
                foreach (var chi in context.Friends)
                {
                    if (AddFriend.Visibility == Visibility.Visible)
                    {
                        if (chi.FfriendId == user.Id)
                        {
                            if (LocateText.Text.Length > 0)
                            {
                                string name = contextUser.UsersInfo.Where(x => x.Id == chi.SfriendId).First().Nikname;
                                if (!(name.Contains(LocateText.Text)))
                                {
                                    continue;
                                }
                            }
                            if (chi.WhoThink != null)
                            {
                                Repuests.Add(chi);
                                continue;
                            }
                        }
                        else if (chi.SfriendId == user.Id)
                        {
                            if(chi.FfriendId == 1)
                            {
                                continue;
                            }
                            if (LocateText.Text.Length > 0)
                            {
                                string name = contextUser.UsersInfo.Where(x => x.Id == chi.FfriendId).First().Nikname;
                                if (!(name.Contains(LocateText.Text)))
                                {
                                    continue;
                                }
                            }
                            if (chi.WhoThink != null)
                            {
                                thinkFriend.Add(chi);
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }

                        myFriends.Add(chi);
                    }
                }

                foreach (var chi in contextUser.UsersInfo)
                {
                    if (LocateText.Text.Length == 0)
                        break;
                    if (chi.Id == 1 || chi.Id == user.Id)
                        continue;

                    if (LocateText.Text.Length > 0)
                    {
                        string name = chi.Nikname;
                        if (name.Contains(LocateText.Text) )
                        {
                            userInfos.Add(chi);
                        }
                    }
                }
            }

            PaintFriend();
        }

        private void PaintFriend()
        {
            int item = LocateBy.SelectedIndex;

            try
            {
                if(AddFriend.Visibility != Visibility.Visible)
                {
                    if(userInfos.Count<=0)
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    if ((myFriends.Count <= 0 &&
                        Repuests.Count <= 0 &&
                        thinkFriend.Count <= 0))
                    {
                        throw new Exception();
                    }
                    else if (item == 1 &&
                        myFriends.Count <= 0)
                    {
                        throw new Exception();
                    }
                    else if (item == 2 &&
                        Repuests.Count <= 0)
                    {
                        throw new Exception();
                    }
                    else if (item == 3 &&
                        thinkFriend.Count <= 0)
                    {
                        throw new Exception();
                    }
                }
            }
            catch
            {
                EmptyText.Visibility = Visibility.Visible;
                return;
            }

            int i;

            using (var context = new SuperCatContext())
            {
                if (AddFriend.Visibility == Visibility.Visible)
                {
                    if (item == 0 || item == 2)
                    {
                        foreach (var fID in Repuests)
                        {
                            if (fID.FfriendId == user.Id)
                            {
                                i = fID.SfriendId;
                            }
                            else
                            {
                                i = fID.FfriendId;
                            }
                            Grid _case = GetCaseForFriend(context.UsersInfo.Where(x => x.Id == i).First());
                            _case = GetWantFriend(_case);

                            AddReadyArea(_case);
                        }
                    }
                    if (item == 0 || item == 3)
                    {
                        foreach (var fID in thinkFriend)
                        {
                            if (fID.FfriendId == user.Id)
                            {
                                i = fID.SfriendId;
                            }
                            else
                            {
                                i = fID.FfriendId;
                            }
                            Grid _case = GetCaseForFriend(context.UsersInfo.Where(x => x.Id == i).First());
                            _case = GetWantFriend(_case);
                            _case = GetThinkFriend(_case);

                            AddReadyArea(_case);
                        }
                    }
                    if (item <= 1)
                    {
                        foreach (var fID in myFriends)
                        {
                            if (fID.FfriendId == user.Id)
                            {
                                i = fID.SfriendId;
                            }
                            else
                            {
                                i = fID.FfriendId;
                            }
                            Grid _case = GetCaseForFriend(context.UsersInfo.Where(x => x.Id == i).First());
                            _case = GetRealyFriend(_case);

                            AddReadyArea(_case);
                        }
                    }

                    return;
                }

                foreach (var fID in userInfos)
                {
                    Grid _case = GetCaseForFriend(context.UsersInfo.Where(x => x.Id == fID.Id).First());
                    _case = GetNewFriend(_case);

                    AddReadyArea(_case);
                }
            }
        }

        private Grid GetNewFriend(Grid _case)
        {
            _case.Children.RemoveAt(4);
            //СДЕСЬ ДОЛЖНО БЫТЬ 4 ПОТОМУ ЧТО ЭЛЕМЕНТОВ 5 НО ОТСЧЕТ С 0 ИДЕОТ!!!

            Image send = new Image
            {
                Stretch = defouldTrashImage.Stretch,
                Cursor = defouldMailBord.Cursor,
                Source = defouldSendImage.Source
            };
            Border sendBord = new Border
            {
                Height = defouldTrashBord.Height,
                Cursor = defouldMailBord.Cursor,
                Width = defouldTrashBord.Width,
                Style = defouldTrashBord.Style,
                Padding = defouldTrashBord.Padding,
                Margin = defouldTrashBord.Margin,
                BorderThickness = defouldTrashBord.BorderThickness,
                BorderBrush = defouldTrashBord.BorderBrush,
            };
            sendBord.MouseDown += SandFriendEmmale;
            sendBord.Child = send;

            _case.Children.Add(sendBord);

            return _case;
        }

        private void LocateText_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key != Key.Enter)
            {
                return;
            }

            Keyboard.ClearFocus();

            GetFriends();
        }
        private void AddFriend_Click(object sender, RoutedEventArgs e)
        {
            MyFriend.Visibility = Visibility.Visible;
            AddFriend.Visibility = Visibility.Collapsed;
            LocateBy.Visibility = Visibility.Collapsed;

            MyFriendArray.Children.Clear();
            LocateText.Text = string.Empty;
            EmptyText.Visibility = Visibility.Visible;
        }
        private void YourFriend_Click(object sender, RoutedEventArgs e)
        {
            AddFriend.Visibility = Visibility.Visible;
            LocateBy.Visibility = Visibility.Visible;
            MyFriend.Visibility = Visibility.Collapsed;

            LocateText.Text = string.Empty;
            FillFriends();
        }

        private void SandFriendEmmale(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            var name = (((sender as Border)!.Parent as Grid)!.Children[1] as Label)!.Content.ToString();
            if(MessageBox.Show("Do you want to send a friend request to \"" + name + "\"?", "Add friend", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            int friendId = Convert.ToInt32((((sender as Border)!.Parent as Grid)!.Children[3] as Label)!.Content);

            using (var context = new SuperCatContext())
            {
                foreach (var fri in context.Friends)
                {
                    if((fri.FfriendId == user.Id && fri.SfriendId == friendId) ||
                        (fri.FfriendId == friendId && fri.SfriendId == user.Id))
                    {
                        MessageBox.Show("You already have a user with the nickname \"" + name + "\" in your friend list.");
                        return;
                    }
                }

                context.Friends.Add(new Friend(user.Id, friendId));

                context.SaveChanges();
            }
        }

        private void defouldFoneBord_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int friendId = Convert.ToInt32((((sender as Border)!.Child as Grid)!.Children[3] as Label)!.Content);
            MyObjects.Friend friendCheck = null!;

            foreach (var fri in myFriends)
            {
                if ((fri.FfriendId == user.Id && fri.SfriendId == friendId) ||
                    (fri.FfriendId == friendId && fri.SfriendId == user.Id))
                {
                    friendCheck = fri;
                    break;
                }
            }

            if (friendCheck == null)
            {
                MessageBox.Show("To view the account, you have to add it as a friend list.");
                return;
            }

            using (var context = new SuperCatContext())
            {
                NavigationService.Navigate(new FriendFile.FriendList(context.UsersInfo.Where(x=>x.Id == friendId).First(), (user.Id == 1)));
            }
        }
    }
}
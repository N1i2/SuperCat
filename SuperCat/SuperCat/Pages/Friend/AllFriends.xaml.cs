using SuperCat.Context;
using SuperCat.GlobalFanc;
using SuperCat.MyObjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace SuperCat.Pages.Friend
{
    /// <summary>
    /// Interaction logic for AllFriends.xaml
    /// </summary>
    public partial class AllFriends : Page
    {
        private UserInfo user = null!;
        private List<MyObjects.Friend> myFriends = null!;
        private List<MyObjects.Friend> thinkFriend = null!;

        public AllFriends()
        {
            InitializeComponent();
        }
        public AllFriends(UserInfo user):this()
        {
            this.user = user;
            myFriends = new List<MyObjects.Friend>();
            thinkFriend = new List<MyObjects.Friend>();

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
            int id = user.ID;
            List<int> friendsID = new List<int>();
            List<int> thinkID = new List<int>();
            List<int> myThinkID = new List<int>();

            using (var context = new SuperCatContext())
            {
                foreach (var chi in context.Friends)
                {
                    if (chi.FirstID == id)
                    {
                        if(chi.Think == null)
                        {
                            friendsID.Add(chi.SecondID);
                        }
                        else
                        {
                            myThinkID.Add(chi.SecondID);
                            continue;
                        }
                    }
                    else if (chi.SecondID == id)
                    {
                        friendsID.Add(chi.FirstID);
                    }
                    else if (chi.Think == id)
                    {
                        thinkID.Add(chi.FirstID);
                        continue;
                    }
                    else
                    {
                        continue;
                    }

                    myFriends.Add(chi);
                }

                if(friendsID.Count <= 0 && 
                    thinkID.Count <= 0 && 
                    myThinkID.Count <= 0)
                {
                    EmptyText.Visibility = Visibility.Visible;
                    return;
                }

                foreach (var fID in friendsID)
                {
                    Grid _case = GetRealyFriend(GetCaseForFriend(context.UsersInfo.Where(x => x.ID == fID).First()));


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

                    MyFriendArray.Children.Add(mainCase);
                }
            }
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
                Content = friend.ID.ToString("D10"),
                FontSize = defouldIdLabel.FontSize,
                Margin = defouldIdLabel.Margin,
            };
            Grid _case = new Grid();
            _case.Children.Add(iconBord);
            _case.Children.Add(nick);
            _case.Children.Add(realN);
            _case.Children.Add(Id);

            return _case;
        }
        private Grid GetRealyFriend(Grid _case)
        {
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
            delBord.MouseUp += defouldTrashBord_Click;
            delBord.Child = del;

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

            _case.Children.Add(delBord);
            _case.Children.Add(mailBord);

            return _case;
        }
        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void AddFriend_Click(object sender, RoutedEventArgs e)
        {

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
            string friendNick = (((sender as Border)!.Parent as Grid)!.Children[1] as Label)!.Content.ToString()??"";

            if (MessageBox.Show(("You want to remove \"" + friendNick + "\" from your friends list"), "Delete", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            int userId = Convert.ToInt32((((sender as Border)!.Parent as Grid)!.Children[3] as Label)!.Content);
            MyObjects.Friend locF = new MyObjects.Friend();

            foreach (var friend in myFriends)
            {
                if((friend.FirstID == user.ID && friend.SecondID == userId) ||
                    ((friend.FirstID == userId && friend.SecondID == user.ID)))
                {
                    locF = friend;
                    break;
                }
            }

            using (var context = new SuperCatContext())
            {
                context.Friends.Remove(locF);
                context.SaveChanges();
            }

            MyFriendArray.Children.Clear();
            myFriends.Clear();

            FillFriends();
        }
    }
}

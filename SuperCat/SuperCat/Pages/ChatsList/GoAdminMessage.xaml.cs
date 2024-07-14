using SuperCat.Context;
using SuperCat.GlobalFanc;
using SuperCat.MyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperCat.Pages.ChatsList
{
    /// <summary>
    /// Interaction logic for GoAdminMessage.xaml
    /// </summary>
    public partial class GoAdminMessage : Page
    {
        private UserInfo user = null!;
        private List<Friend> chine = null!;
        private List<int> messages = null!;
        private Border LocateMess = null!;

        public GoAdminMessage()
        {
            InitializeComponent();
            messages = new List<int>();
            chine = new List<Friend>();

            using (var context = new SuperCatContext())
            {
                user = context.UsersInfo.First(x => x.Id == 1);

                List<Friend> users = context.Friends.Where(x => x.FfriendId == 1).ToList();

                foreach (var i in users)
                {
                    if (i.Messages == "")
                        continue;

                    messages.AddRange(HelpWork.UnboxStringByList(i.Messages));
                    chine.Add(i);
                }
            }

            FillMessage();
        }
        private void FillMessage()
        {
            using (var context = new SuperCatContext())
            {
                foreach (var mess in messages)
                {
                    ChatInfo chat = context.ChatsInfo.Where(x => x.Id == mess).First();

                    ShowMessage(chat.WhoSay,
                        context.UsersInfo.Where(x => x.Id == chat.WhoSay).First().Nikname,
                        chat.WhenSay, chat.WhatSay, chat.Id);
                }
            }
        }
        private void ShowMessage(int owner, string WhoSay, string WhenSay, string WhatSay, int id)
        {
            var textBlock = new TextBlock
            {
                Text = WhatSay,
                FontSize = 10,
                Margin = new Thickness(15, 0, 15, 0),
                TextWrapping = TextWrapping.Wrap
            };
            var textName = new TextBlock
            {
                Text = WhoSay,
                FontSize = 8,
                Foreground = Brushes.Gray,
                Margin = new Thickness(15, 5, 15, 5),
                TextTrimming = TextTrimming.WordEllipsis
            };
            var textDate = new TextBlock
            {
                Text = WhenSay,
                FontSize = 8,
                Foreground = Brushes.Gray,
                Margin = new Thickness(15, 5, 15, 5),
                TextTrimming = TextTrimming.WordEllipsis
            };
            var textId = new TextBlock
            {
                Visibility = Visibility.Collapsed,
                Text = id.ToString()
            };

            StackPanel _case = new StackPanel();

            _case.Children.Add(textId);
            _case.Children.Add(textName);
            _case.Children.Add(textBlock);
            _case.Children.Add(textDate);

            var border = new Border
            {
                MaxWidth = 400,
                CornerRadius = new CornerRadius(20),
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(50, 3, 50, 3),
                Child = _case,
                Effect = new DropShadowEffect
                {
                    Color = Colors.LightGray,
                    Direction = 315,
                    ShadowDepth = 3,
                    Opacity = 1,
                    BlurRadius = 7
                }
            };

            if (owner == user.Id)
            {
                border.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                border.Background = Brushes.LightBlue;
            }
            else
            {
                border.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                border.Background = Brushes.LightGray;
            }
            border.MouseEnter += Message_MouseEnter;
            border.MouseLeave += Message_MouseLeave;

            MessagesPanel.Children.Add(border);
        }

        private void BackClick(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            Border delElem = LocateMess;

            if (e.Key != Key.Delete || LocateMess == null)
            {
                return;
            }

            string what = ((LocateMess.Child as StackPanel)!.Children[2] as TextBlock)!.Text;
            int id = Convert.ToInt32(((LocateMess.Child as StackPanel)!.Children[0] as TextBlock)!.Text);

            if (MessageBox.Show($"You want to delite massage \"{what}\"", "Delete Message", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            using (var contextChat = new SuperCatContext())
            using (var contextFriend = new SuperCatContext())
            {
                contextChat.ChatsInfo.Remove(contextChat.ChatsInfo.First(x => x.Id == id));

                Friend ch = null!;

                foreach (var item in chine)
                {
                    if (HelpWork.UnboxStringByList(item.Messages).Contains(id))
                    {
                        ch = item;
                        break;
                    }
                }

                contextFriend.Friends.First(x => x.Id == ch.Id).Messages = HelpWork.UnboxStringByString(
                    contextFriend.Friends.First(x => x.Id == ch.Id).Messages, id);

                contextChat.SaveChanges();
                contextFriend.SaveChanges();
            }

            MessagesPanel.Children.Remove(delElem);

            Keyboard.ClearFocus();
        }

        private void Message_MouseEnter(object sender, MouseEventArgs e)
        {
            LocateMess = (sender as Border)!;
        }
        private void Message_MouseLeave(object sender, MouseEventArgs e)
        {
            LocateMess = null!;
        }

    }
}

using Microsoft.EntityFrameworkCore.Diagnostics;
using SuperCat.Context;
using SuperCat.GlobalFanc;
using SuperCat.MyObjects;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace SuperCat.Pages.ChatsList
{
    /// <summary>
    /// Interaction logic for GoChat.xaml
    /// </summary>
    public partial class GoChat : Page
    {
        private UserInfo user = null!;
        private UserInfo friend = null!;
        private Friend chine = null!;
        private List<int> messages = null!;
        private Border LocateMess = null!;
        public GoChat()
        {
            InitializeComponent();
        }
        public GoChat(int youId, int friendId, string messages) : this()
        {
            this.user = GetUsersById(youId);
            this.friend = GetFriendById(friendId);
            this.messages = GetMessage(messages);
            PaintName();

            FillMessage();
        }
        private void PaintName()
        {
            if (friend == null)
            {
                BackClick(null!, null!);
            }

            Run run = new Run(Application.Current.Resources["YouFriendTex"] as string);

            Run run2 = new Run(" " + friend!.Nikname);
            run2.Foreground = Brushes.Orange;

            HeaderText.Inlines.Add(run);
            HeaderText.Inlines.Add(run2);
        }
        private List<int> GetMessage(string mes)
        {
            List<int> arr = new List<int>();

            foreach (var m in mes.Split("."))
            {
                if (int.TryParse(m, out int i))
                {
                    arr.Add(i);
                }
            }

            return arr;
        }
        private void FillMessage()
        {
            using (var context = new SuperCatContext())
            {
                foreach (var mess in messages)
                {
                    ChatInfo chat = context.ChatsInfo.Where(x => x.Id == mess).First();

                    ShowMessage(chat.WhoSay,
                        context.UsersInfo.Where(x=>x.Id == chat.WhoSay).First().Nikname,
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
        private int CreateOneMessage(int WhoSay, string WhenSay, string WhatSay)
        {
            if (messages.Count >= 200)
            {
                messages.RemoveAt(0);
                MessagesPanel.Children.RemoveAt(0);
            }

            var message = new ChatInfo(WhoSay, WhenSay, WhatSay, chine.Id, null!);

            using (var context = new SuperCatContext())
            using (var contextFriend = new SuperCatContext())
            {
                context.ChatsInfo.Add(message);
                context.SaveChanges();

                List<ChatInfo> id = context.ChatsInfo.Where(x=>x.WhatSay == message.WhatSay).ToList();
                id = id.Where(x => x.WhoSay == message.WhoSay).ToList();
                id = id.Where(x => x.WhenSay == message.WhenSay).ToList();

                messages.Add(id.Last().Id);

                contextFriend.Friends.Where(x => x.Id == chine.Id).First().Messages = ConvertStringByList();
                contextFriend.SaveChanges();

                return id.Last().Id;
            }
        }
        private string ConvertStringByList()
        {
            string mes = string.Empty;

            foreach (var item in messages)
            {
                mes += "." + item.ToString();
            }

            return mes;
        }
        private UserInfo GetUsersById(int id)
        {
            using (var context = new SuperCatContext())
            {
                return context.UsersInfo.Where(x => x.Id == id).FirstOrDefault()!;
            }
        }
        private UserInfo GetFriendById(int id)
        {
            using (var context = new SuperCatContext())
            {
                chine = context.Friends.Where(x => x.Id == id).First();

                if (chine.FfriendId == user.Id)
                    return context.UsersInfo.Where(x => x.Id == chine.SfriendId).First();
                return context.UsersInfo.Where(x => x.Id == chine.FfriendId).First();
            }
        }
        private void BackClick(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e != null)
            {
                e.Handled = true;
            }

            YourTextMessage.Text = YourTextMessage.Text.TrimEnd();
            YourTextMessage.Text = YourTextMessage.Text.TrimStart();
            YourTextMessage.Text = Regex.Replace(YourTextMessage.Text, @"\s{2,}", " ");

            if (YourTextMessage.Text.Length <= 0)
            {
                YourTextMessage.Text = string.Empty;
                return;
            }
            if(YourTextMessage.Text.Length >= 500)
            {
                MessageBox.Show("The message should not exceed 500 characters");
                Keyboard.ClearFocus();
                return;
            }

            int id = CreateOneMessage(user.Id, GetCurrentDateTime(), YourTextMessage.Text);
            ShowMessage(user.Id, user.Nikname, GetCurrentDateTime(), YourTextMessage.Text, id);

            Write.Visibility = Visibility.Visible;
            YourTextMessage.Text = string.Empty;

            SpeakArea.ScrollToBottom();
        }
        public string GetCurrentDateTime()
        {
            DateTime now = DateTime.Now;
            //return $"{now.Year}, {now.Month}, {now.Hour}, {now.Minute}";
            string mounth = (now.Month.ToString().Length == 1) ? ("0" + now.Month) : (now.Month.ToString());
            string Minute = (now.Minute.ToString().Length == 1)? ("0" + now.Minute) : (now.Minute.ToString());
            return $"{now.Year},{mounth} - {now.Hour}:{Minute}";
        }
        private void TextMessage_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(YourTextMessage.Text.Replace(" ", "").Length <= 0)
            {
                Write.Visibility = Visibility.Visible;
            }
            else
            {
                Write.Visibility = Visibility.Collapsed;
            }

            if (e.Key != System.Windows.Input.Key.Enter)
            {
                return;
            }

            Image_MouseDown(null!, null!);
        }

        private void Write_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            YourTextMessage.Focus();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            SpeakArea.ScrollToBottom();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            Border delElem = LocateMess;

            if(e.Key != Key.Delete || LocateMess == null)
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

                contextFriend.Friends.First(x => x.Id == chine.Id).Messages = HelpWork.UnboxStringByString(
                    contextFriend.Friends.First(x => x.Id == chine.Id).Messages, id);

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

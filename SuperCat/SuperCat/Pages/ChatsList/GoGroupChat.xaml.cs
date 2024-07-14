using SuperCat.Context;
using SuperCat.GlobalFanc;
using SuperCat.MyObjects;
using SuperCat.Pages.Group;
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
    /// Interaction logic for GoGroupChat.xaml
    /// </summary>
    public partial class GoGroupChat : Page
    {
        private UserInfo user = null!;
        private GroupInfo thisGroup = null!;
        private List<int> messages = null!;
        private Border LocateMess = null!;

        public GoGroupChat()
        {
            InitializeComponent();
        }
        public GoGroupChat(int youId, GroupInfo group) : this()
        {
            this.user = GetUsersById(youId);
            this.thisGroup = group;
            this.messages = HelpWork.UnboxStringByList(group.Messages);

            if (user.Id == group.OwnerId || user.Id == 1)
            {
                SettingButon.Visibility = Visibility.Visible;
            }

            PaintName();
            FillMessage();
        }

        private void PaintName()
        {
            HeaderText.Inlines.Clear();

            Run run = new Run(Application.Current.Resources["YouGroupTex"] as string);

            Run run2 = new Run(" " + thisGroup.Name);
            run2.Foreground = Brushes.Orange;

            HeaderText.Inlines.Add(run);
            HeaderText.Inlines.Add(run2);
        }

        private void FillMessage()
        {
            using (var context = new SuperCatContext())
            {
                foreach (var mess in messages)
                {
                    ChatInfo chat = context.ChatsInfo.Where(x => x.Id == mess).First();

                    ShowMessage(chat.WhoSay,
                        context.UsersInfo.First(x => x.Id == chat.WhoSay).Nikname,
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

            var message = new ChatInfo(WhoSay, WhenSay, WhatSay, null!, thisGroup.Id);

            using (var context = new SuperCatContext())
            using (var contextGroup = new SuperCatContext())
            {
                context.ChatsInfo.Add(message);
                context.SaveChanges();

                List<ChatInfo> id = context.ChatsInfo.Where(x => x.WhatSay == message.WhatSay).ToList();
                id = id.Where(x => x.WhoSay == message.WhoSay).ToList();
                id = id.Where(x => x.WhenSay == message.WhenSay).ToList();

                messages.Add(id.Last().Id);

                contextGroup.GroupsInfo.First(x => x.Id == thisGroup.Id).Messages = ConvertStringByList();
                contextGroup.SaveChanges();

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
                return context.UsersInfo.FirstOrDefault(x => x.Id == id)!;
            }
        }

        private void BackClick(object sender, RoutedEventArgs e)
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
            if (YourTextMessage.Text.Length >= 500)
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
            string Minute = (now.Minute.ToString().Length == 1) ? ("0" + now.Minute) : (now.Minute.ToString());
            return $"{now.Year},{mounth} - {now.Hour}:{Minute}";
        }

        private void TextMessage_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (YourTextMessage.Text.Replace(" ", "").Length <= 0)
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

        private void Write_MouseDown(object sender, MouseButtonEventArgs e)
        {
            YourTextMessage.Focus();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new SuperCatContext())
            {
                user = context.UsersInfo.First(x => x.Id == user.Id);
                thisGroup = context.GroupsInfo.First(x => x.Id == thisGroup.Id);
            }

            PaintName();

            SpeakArea.ScrollToBottom();
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
            using (var contextGroup = new SuperCatContext())
            {
                contextChat.ChatsInfo.Remove(contextChat.ChatsInfo.First(x => x.Id == id));

                contextGroup.GroupsInfo.First(x => x.Id == thisGroup.Id).Messages = HelpWork.UnboxStringByString(
                    contextGroup.GroupsInfo.First(x => x.Id == thisGroup.Id).Messages, id);

                contextChat.SaveChanges();
                contextGroup.SaveChanges();
            }

            MessagesPanel.Children.Remove(delElem);

            Keyboard.ClearFocus();
        }
        private void GoSettings(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new CreateNewGroup(user, thisGroup));
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

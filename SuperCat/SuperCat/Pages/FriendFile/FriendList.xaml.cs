using Microsoft.Win32;
using SuperCat.Context;
using SuperCat.MyObjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using SuperCat.GlobalFanc;
using System.Windows.Input;
using SuperCat.Lists;


namespace SuperCat.Pages.FriendFile
{
    /// <summary>
    /// Interaction logic for MyList.xaml
    /// </summary>
    public partial class FriendList : Page
    {
        private UserInfo user;
        private List<MyImage> images;
        private bool admin;
        
        public FriendList()
        {
            InitializeComponent();
            user = new UserInfo();
            images = new List<MyImage>(); 
        }
        public FriendList(UserInfo user, bool admin):this()
        {
            this.user = user;
            this.admin = admin;

            if (admin)
            {
                AddImageButton.Visibility = Visibility.Visible;
                SettingsButton.Visibility = Visibility.Visible;
            }

            CatImage.Source = HelpWork.LoadImageFromByte(this.user.Image??new byte[0]);
            FillList();
            PaintImages();
        }

        private void PaintImages()
        {
            using (var context = new SuperCatContext())
            {
                images = context.MyImages.Where(x => x.UserInfoId == user.Id).ToList();
            }

            foreach (var img in images)
            {
                ImageArea.Items.Insert(1, img.Image);
            }

            if(!admin && images.Count <= 0) 
            {
                emptyImage.Visibility = Visibility.Visible;
            }
        }
        private void FillList()
        {
            if(user.Nikname != null)
                nicknameBox.Content = user.Nikname;
            if(user.RealName != null)
                realNameBox.Content = user.RealName;
            if (user.Gender != null)
                genderBox.Content = (user.Gender == "m")?"Man":"Woman";
            if (user.Email != null)
                emailBox.Content = user.Email;
            if (user.Birthday != null)
                YearsBox.Content = user.Birthday;
        }

        private void GoSettings(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Settings(user));
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "PNG Images (*.png)|*.png",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var imageBytes = HelpWork.GetBytesImageSource(new BitmapImage(new Uri(openFileDialog.FileName)));

                using (var context = new SuperCatContext())
                {
                    context.MyImages.Add(new MyImage(imageBytes, user.Id));

                    context.SaveChanges();
                }


                ImageArea.Items.Insert(1, imageBytes);
            }
        }

        private void ImageArea_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Image image && image != ImageArea.Items[0])
            {
                FullImage.Source = image.Source;
                ImageFoun.Visibility = Visibility.Visible;
                ImageBoreder.Visibility = Visibility.Visible;
                CloseFullImage.Visibility = Visibility.Visible;
                DeleteImage.Visibility = Visibility.Visible;
                ImageFoun.Focus();
            }
        }

        private void CloseFullImage_Click(object sender, RoutedEventArgs e)
        {
            ImageFoun.Visibility = Visibility.Collapsed;
            ImageBoreder.Visibility = Visibility.Collapsed;
            CloseFullImage.Visibility = Visibility.Collapsed;
            DeleteImage.Visibility = Visibility.Collapsed;
        }

        private void DeleteImage_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove this image?", "hello", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                DeleteImage.Focus();

                return;
            }
            DeleteImage.Focus();


            using (var context = new SuperCatContext())
            {
                byte[] imageBytes = HelpWork.GetBytesImageSource(FullImage.Source);

                var userImages = context.MyImages.Where(x => x.UserInfoId == user.Id).ToList();
                var imageToDelete = userImages.First(x => x.Image.SequenceEqual(imageBytes));

                if (imageToDelete != null)
                {
                    context.MyImages.Remove(imageToDelete);
                    context.SaveChanges();

                    var imageToRemove = ImageArea.Items.OfType<Image>().FirstOrDefault(x => ((BitmapImage)x.Source).UriSource == ((BitmapImage)FullImage.Source).UriSource);
                    if (imageToRemove != null)
                    {
                        ImageArea.Items.Remove(imageToRemove);
                    }
                }
            }

            ImageArea.Items.Clear();
            Button but = new Button
            {
                Name = "AddImageButton",
                Content = System.Windows.Application.Current.Resources["AddImageText"] as string,
                Margin = new Thickness(10),
                Cursor = Cursors.Hand,
                Width = 100,
                Height = 100
            };
            but.Click += AddImage_Click;
            ImageArea.Items.Insert(0, but);

            PaintImages();

            CloseFullImage_Click(sender, e);
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(!(DeleteImage.Visibility == Visibility.Visible
                && (e.Key == System.Windows.Input.Key.Delete 
                || e.Key == System.Windows.Input.Key.Escape)))
            {
                return;
            }

            if(e.Key == System.Windows.Input.Key.Delete)
            {
                DeleteImage_Click(sender, e);
            }
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                CloseFullImage_Click(sender, e);
            }
        }
        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
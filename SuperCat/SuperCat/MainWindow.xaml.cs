using SuperCat.Log;
using System.Windows;

namespace SuperCat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Content = new LogPage();
        }
    }
}
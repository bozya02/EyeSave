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
using EyeSave.Pages;

namespace EyeSave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EyeSaveWindow : Window
    {
        public EyeSaveWindow()
        {
            InitializeComponent();
            frame.NavigationService.Navigate(new AgentsListPage());
            frame.Navigated += Frame_Navigated;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            var content = frame.Content as Page;

            if (content is AgentsListPage)
            {
                btnBack.Visibility = Visibility.Collapsed;
                btnForward.Visibility = Visibility.Visible;
            }
            else
            {
                btnBack.Visibility = Visibility.Visible;
                btnForward.Visibility = Visibility.Collapsed;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (frame.CanGoBack)
                frame.GoBack();
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (frame.CanGoForward)
                frame.GoForward();
        }
    }
}

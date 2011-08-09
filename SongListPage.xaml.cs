using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace MegaStarzWP7
{
    public partial class SongListPage : PhoneApplicationPage
    {
        public SongListPage()
        {
            DataContext = ((App) Application.Current).SongList;
            InitializeComponent();
        }

        /// <summary>
        /// Raised when song is selected and navigate to the karaoke page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedSongChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/KaraokePage.xaml", UriKind.Relative));
        }
    }
}
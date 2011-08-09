using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;

namespace MegaStarzWP7
{
    public partial class KaraokePage : PhoneApplicationPage
    {

        #region CTOR

        public KaraokePage()
        {
            DataContext = ((App) Application.Current).SongList;
            InitializeComponent();
        }

        #endregion

        //TODO: bind the video source to the video player

        private void OnFuncButtonClick(object sender, RoutedEventArgs e)
        {
            if ((karaokePlayer.CurrentState == MediaElementState.Stopped) || (karaokePlayer.CurrentState == MediaElementState.Paused)
                || (karaokePlayer.CurrentState == MediaElementState.Opening))
            {
                karaokePlayer.Play();
            }
            else if (karaokePlayer.CurrentState == MediaElementState.Playing)
            {
                karaokePlayer.Stop();
            }
            else
            {
                MessageBox.Show("Undeclared Video Player State");
            }
        }

    }
}
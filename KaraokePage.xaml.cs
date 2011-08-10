using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Media;
using MegaStarzWP7.Models;
using MegaStarzWP7.ViewModels;
using Microsoft.Phone.Controls;

namespace MegaStarzWP7
{
    public partial class KaraokePage : PhoneApplicationPage
    {

        private MegaStarzViewModels songList;
        private IsolatedStorageFileStream fileStream;

        #region CTOR

        public KaraokePage()
        {
            DataContext = ((App) Application.Current).SongList;
            songList = ((App)Application.Current).SongList;
            InitializeComponent();
        }

        #endregion

        //TODO: bind the video source to the video player

        private void OnFuncButtonClick(object sender, RoutedEventArgs e)
        {
            if ((karaokePlayer.CurrentState == MediaElementState.Stopped) || (karaokePlayer.CurrentState == MediaElementState.Paused)
                || (karaokePlayer.CurrentState == MediaElementState.Opening) || karaokePlayer.CurrentState == MediaElementState.Closed)
            {
                fileStream = FilesManager.OpenSongStream(songList.SelectedSong.SongURI);

                karaokePlayer.SetSource(fileStream);
                karaokePlayer.Play();
            }
            else if (karaokePlayer.CurrentState == MediaElementState.Playing)
            {
                fileStream.Close();
                fileStream.Dispose();
                karaokePlayer.Stop();
            }
            else
            {
                MessageBox.Show("Undeclared Video Player State");
            }
        }

    }
}
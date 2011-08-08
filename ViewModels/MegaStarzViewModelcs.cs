using System.Collections.ObjectModel;
using System.ComponentModel;
using MegaStarzWP7.Models;

namespace MegaStarzWP7.ViewModels
{
    public class MegaStarzViewModelcs : INotifyPropertyChanged
    {
        #region Properties

        private ObservableCollection<SongModel> songs;
        public ObservableCollection<SongModel> Songs
        {
            get
            {
                return songs;
            }
            set
            {
                if (value != songs)
                {
                    songs = value;
                    NotifyPropertyChanged("Songs");
                }
            }
        }


        private SongModel selectedSongs;
        public SongModel SelectedSongs
        {
            get
            {
                return selectedSongs;
            }
            set
            {
                if (value != selectedSongs)
                {
                    selectedSongs = value;
                    NotifyPropertyChanged("SelectedSongs");
                }
            }
        }

        #endregion

        #region CTOR

        public MegaStarzViewModelcs()
        {
            songs = new ObservableCollection<SongModel>();
        }

        #endregion

                #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(property));
        }

        #endregion

    }
}

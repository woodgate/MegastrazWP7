using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MegaStarzWP7.Models;
using Megastar.Client.Library;
using Megastar.RestServices.Library.Entities;

namespace MegaStarzWP7.ViewModels
{
    public class MegaStarzViewModels : INotifyPropertyChanged
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

        public MegaStarzViewModels()
        {
            songs = new ObservableCollection<SongModel>();
        }

        #endregion

        #region Load Methods
        /// <summary>
        /// Start the process of Loading the songs from the server
        /// </summary>
        public void LoadSongs()
        {
            MegaStarzClient client = new MegaStarzClient();

            //Get song list from server
            try
            {
                client.GetSongsAsync((result) =>
                                         {

                                             if (result != null)
                                                 LoadSongList(result);

                                         });
            }
            catch (Exception e)
            {
                throw e; //TODO: Handle Error
            }
        }

        /// <summary>
        /// This function loads song list into songs Collection.
        /// </summary>
        private void LoadSongList(List<SongResponse> songsResponse)
        {
            foreach (var s in songsResponse)
            {
                bool exists = SongManager.CheckIfSongIsLoaded(s.id);
                //Download song from server with DownloadSongComplete callback

                //Create corresponding SongModel
                var songModel = new SongModel(s.id, s.artistName, s.name, LengthToString(s.length), string.Empty,
                                              new Uri(s.playUrl), exists); //TODO: Get Song Picture

                songs.Add(songModel);
            }
        }


        private string LengthToString(int length)
        {
            var timeSpan = new TimeSpan(0, 0, 0, length);

            //TODO: Nitzan, This throw "Input string was not in a correct format." exception
            return String.Empty;
//            return timeSpan.ToString("{c}");
        }

        #endregion

        #region IsolatedStorage Methods

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

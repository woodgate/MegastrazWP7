using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows;
using MegaStarzWP7.Models;
using Megastar.Client.Library;
using Megastar.RestServices.Library.Entities;

namespace MegaStarzWP7.ViewModels
{
    public class MegaStarzViewModels : INotifyPropertyChanged
    {
        #region CTOR

        public MegaStarzViewModels()
        {
            songs = new ObservableCollection<SongModel>();
            //TODO: check in the settings if it is the first time
            //FirstTimeInitialization();
        }

        #endregion

        #region First Time Intioalization Method

        private void FirstTimeInitialization()
        {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    store.CreateDirectory("Videos");
                    //create first song
                    songs.Add(new SongModel(0, "Abba", "Mamma Mia", "00:00", "", null, true));
                    //TODO: store the first song
                    //create second song
                    songs.Add(new SongModel(0, "Abba", "The Winner Takes It All", "00:00", "", null, true));
                    //TODO: store the seconde song
                }
        }
        
        #endregion

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

       
        private SongModel selectedSong;

        public SongModel SelectedSong
        {
            get
            {
                return selectedSong;
            }
            set
            {
                if (value != selectedSong)
                {
                    selectedSong = value;
                    NotifyPropertyChanged("SelectedSong");
                }
            }
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
                MessageBox.Show("Could not get song list from server");
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

                //if (exists) //TODO: Remove this later
                //{
                    //Create corresponding SongModel
                    var songModel = new SongModel(s.id, s.artistName, s.name, LengthToString(s.length), string.Empty,
                                                  new Uri(s.playUrl), exists); //TODO: Get Song Picture

                    songs.Add(songModel);
                //}

                
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

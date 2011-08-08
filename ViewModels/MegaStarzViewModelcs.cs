using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using MegaStarzWP7.Models;
using Megastar.Client.Library;
using Megastar.RestServices.Library.Entities;

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


        #region Private fields

        //Songs That have not been loaded from server
        private List<SongResponse> unloadedSongList; 

        #endregion
        #region CTOR

        public MegaStarzViewModelcs()
        {
            songs = new ObservableCollection<SongModel>();

            MegaStarzClient client = new MegaStarzClient();

            //Get song list from server
            try
            {
                client.GetSongsAsync((result) =>
                                         {
                                             unloadedSongList = result;

                                             if (result != null)
                                                 LoadNextSongFromServer();

                                         });
            }
            catch (Exception e)
            {
                throw e; //TODO: Handle Error
            }


        }

        /// <summary>
        /// This function starts a download of the next song from the server
        /// </summary>
        private void LoadNextSongFromServer()
        {
            if (unloadedSongList.Count > 0)
            {
                var song = unloadedSongList[0];
                unloadedSongList.RemoveAt(0);

                //Download song from server with DownloadSongComplete callback
                SongManager.DownloadAndSaveSongAsync(song, DownloadSongComplete);
            }
        }

        private void DownloadSongComplete(SongResponse song)
        {
            var s = new SongModel(song.id, song.artistName, song.name, LengthToString(song.length), null, song.playUrl);

            if (ShouldDownloadNextSong())
                LoadNextSongFromServer();
        }

        private bool ShouldDownloadNextSong()
        {
            return false;       //TODO: Download next song logic
        }

        private string LengthToString(int length)
        {
            var timeSpan = new TimeSpan(0,0,0,length);

            return timeSpan.ToString("{c}");
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

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using MegaStarzWP7.ViewModels;

namespace MegaStarzWP7.Models
{
    public class SongModel : INotifyPropertyChanged
    {
                #region CTOR

        public SongModel(int id, string artist, string name, string length, string pictureURI, Uri serverURI, bool isLoaded)
        {
            this.id = id;
            this.artist = artist;
            this.name = name;
            this.length = length;
            this.pictureURI = pictureURI;
            this.isLoaded = isLoaded;
            this.serverURI = serverURI;
        }

        #endregion

        public void LoadSong()
        {
            if (!isLoaded)
            {
                SongManager.DownloadAndSaveSongAsync(this);
            }
        }

        #region Properties

        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    NotifyPropertyChanged("Id");
                }

            }
        }

        private string artist;
        public string Artist
        {
            get
            {
                return artist;
            }
            set
            {
                if (value != artist)
                {
                    artist = value;
                    NotifyPropertyChanged("Artist");
                }
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string length;
        public string Length
        {
            get
            {
                return length;
            }
            set
            {
                if (value != length)
                {
                    length = value;
                    NotifyPropertyChanged("Length");
                }
            }
        }

        private string pictureURI;
        public string PictureURI
        {
            get
            {
                return pictureURI;
            }
            set
            {
                if (value != pictureURI)
                {
                    pictureURI = value;
                    NotifyPropertyChanged("PictureURI");
                }
            }
        }

        private string songURI;
        public string SongURI
        {
            get
            {
                return SongManager.fileDirectory + id.ToString();
            }
            set { //DoNothing
                }
        }

        private Uri serverURI;
        public Uri ServerURI
        {
            get { return serverURI; }
            set
            {
                if (value != serverURI)
                {
                    serverURI = value;
                    NotifyPropertyChanged("ServerURI");
                }
            }
        }

        private bool isLoaded;
        public bool IsLoaded
        {
            get
            {
                return isLoaded;
            }

            set
            {
                if (value != isLoaded)
                {
                    isLoaded = value;
                    NotifyPropertyChanged("IsLoaded");
                }
            }
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

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

namespace MegaStarzWP7.Models
{
    public class SongModel : INotifyPropertyChanged
    {
                #region CTOR

        public SongModel(string artist, string name, string length, string pictureURI, string songURI)
        {
            this.artist = artist;
            this.name = name;
            this.length = length;
            this.pictureURI = pictureURI;
            this.songURI = songURI;
        }

        #endregion


        #region Properties

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
                return songURI;
            }
            set
            {
                if (value != songURI)
                {
                    songURI = value;
                    NotifyPropertyChanged("SongURI");
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

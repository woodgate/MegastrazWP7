﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using MegaStarzAudioPlaybackAgent;
using MegaStarzWP7.ViewModels;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Controls;


namespace MegaStarzWP7
{
    public partial class KaraokePage : PhoneApplicationPage
    {

        #region Members

        private FileSink sink;
        private CaptureSource captureSource;
        private string m_capturedFileName = "myVideo.mp4";
        private MegaStarzViewModels megaStarzViewModels;
        private bool startRecored;
        #endregion

        #region CTOR

        public KaraokePage()
        {
            megaStarzViewModels = ((App)Application.Current).MegaStarzViewModelInstance;
            DataContext = megaStarzViewModels;
            InitializeComponent();
            BackgroundAudioPlayer.Instance.PlayStateChanged += new EventHandler(Instance_PlayStateChanged);
            InitializeCaptureDevice();
            startRecored = false;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Initialize the Capture Device
        /// </summary>
        private void InitializeCaptureDevice()
        {
            //set the video capture device
            if (captureSource == null)
            {
                captureSource = new CaptureSource();
                captureSource.VideoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();
                captureSource.AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice();

                sink = new FileSink();
                sink.CaptureSource = captureSource;
                sink.IsolatedStorageFileName = m_capturedFileName;
            }

            //set the video preview
            var brush = new VideoBrush();
            brush.SetSource(captureSource);
            cameraPreview.Fill = brush;
        }

        /// <summary>
        /// Set the song the the selected song
        /// Stop the BackgroundAudioPlayer in case it's playing while navigate into the page
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
//            if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState)
//            {
//                BackgroundAudioPlayer.Instance.Stop();
//            }

            SelectedTrack.SelectedSongUri = new Uri(megaStarzViewModels.SelectedSong.SongURI,UriKind.Relative);
        }

        #endregion

        private void OnFuncButtonClick(object sender, RoutedEventArgs e)
        {
            //case recording stop the recorde
            if ((PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState) && startRecored)
            {
                BackgroundAudioPlayer.Instance.Stop();
            }
            //case record stopped after recording
            else if ((PlayState.Stopped == BackgroundAudioPlayer.Instance.PlayerState)&& startRecored)
            {
                //TODO: share the songe
                NavigationService.Navigate(new Uri("/SongListPage.xaml", UriKind.Relative));
            }
            //case the record didn't started yet
            else
            {
                captureSource.Start();
                BackgroundAudioPlayer.Instance.Play();
            }
        }

        #region BackGroundPlayer Methods

        /// <summary>
        /// changing the button state while the BackgroundAudioPlayer change his state
        /// </summary>
        void Instance_PlayStateChanged(object sender, EventArgs e)
        {
            switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                //case song started to play
                case PlayState.Playing:
                    funcBtn.Style = (Style) Resources["stopButton"];
                    //TODO: start animat the lyrics
                    break;
                //case the song endded or stoped
                case PlayState.Paused:
                case PlayState.Stopped:
                    funcBtn.Style = (Style)Resources["shareButton"];
                    //TODO: stop animat the lyrics
                    break;
            }
        }
        
        #endregion


    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
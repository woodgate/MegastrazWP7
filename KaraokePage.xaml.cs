﻿using System.Windows;
using System.Windows.Media;
using MegaStarzWP7.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MegaStarzWP7
{
    public partial class KaraokePage : PhoneApplicationPage
    {

        #region Members

        private FileSink sink;
        private CaptureSource captureSource;
        private string m_capturedFileName = "myVideo.mp4";
        private SoundEffect _kareokeSong;


        #endregion

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
            FrameworkDispatcher.Update();
            var stream = FilesManager.GetFileStream("Megastarz\\test.wav");


            _kareokeSong = SoundEffect.FromStream(stream);
            _kareokeSong.Play(1.0f, 0.0f, 0.0f);

            //if ((karaokePlayer.CurrentState == MediaElementState.Stopped) || (karaokePlayer.CurrentState == MediaElementState.Paused)
            //    || (karaokePlayer.CurrentState == MediaElementState.Opening) || karaokePlayer.CurrentState == MediaElementState.Closed)
            {
                if (captureSource == null)
                {
                    captureSource = new CaptureSource();
                    captureSource.VideoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();
                    //captureSource.AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice();

                    sink = new FileSink();
                    sink.CaptureSource = captureSource;
                    sink.IsolatedStorageFileName = m_capturedFileName;
                }

                //set the video preview
                VideoBrush brush = new VideoBrush();
                brush.SetSource(captureSource);
                cameraPreview.Fill = brush;
                
                //start capture and to show video
                captureSource.Start();
                //karaokePlayer.Play();

             



            }
            //else if (karaokePlayer.CurrentState == MediaElementState.Playing)\\\
            //{
            //    //karaokePlayer.Stop();
            //    captureSource.Stop();
            //}
            //else
            //{
            //    MessageBox.Show("Undeclared Video Player State");
            //}
        }

    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MegaStarzWP7.ViewModels;
using Megastar.Client.Library;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Controls;


namespace MegaStarzWP7
{
    public partial class KaraokePage : PhoneApplicationPage
    {

        #region Members

        private FileSink sink;
        private CaptureSource captureSource;
        private string m_capturedFileName = "MegaStarz\\myVideo.mp4";
        private MegaStarzViewModels megaStarzViewModels;
        private bool startRecored;
        #endregion

        #region CTOR

        public KaraokePage()
        {
            megaStarzViewModels = ((App)Application.Current).MegaStarzViewModelInstance;
            DataContext = megaStarzViewModels;
            InitializeComponent();
            startRecored = false;
            Loaded += KaraokePage_Loaded;
        }

        void KaraokePage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCaptureDevice();
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

        #endregion

        private void OnFuncButtonClick(object sender, RoutedEventArgs e)
        {
            //case recording stop the recorde
            if (!startRecored)
            {
                funcBtn.Style = (Style)Resources["stopButton"];
                startRecored = true;
                captureSource.Start();
                var animation = (Storyboard)Resources["lyricsAnimation"];
                animation.Begin();
            }
            //case record stopped after recording
            else if(startRecored && (captureSource.State == CaptureState.Stopped))
            {
                

                MessageBox.Show("Please wait while song is beign shared");

                var client = new MegaStarzClient();

                var t = ((App) (App.Current)).starTicket;

                var stream = FilesManager.GetFileStream("MegaStarz\\myVideo.mp4");
                
                    client.UploadRecordingAsync(t, 3, stream, (response) =>
                                                                  {
                                                                      stream.Close();
                                                                      stream.Dispose();

                                                                      if (response != null)
                                                                      {
                                                                          Dispatcher.BeginInvoke(() =>
                                                                                                     {
                                                                                                         MessageBox.Show
                                                                                                             ("Uploaded!");
                                                                                                     });
                                                                      }
                                                                      
                                                                  });
                
                
            }
            //case the record didn't started yet
            else
            {
                funcBtn.Style = (Style)Resources["shareButton"];
                captureSource.Stop();
                var animation = (Storyboard)Resources["lyricsAnimation"];
                animation.Stop();
            }
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
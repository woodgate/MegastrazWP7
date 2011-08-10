﻿using System.Windows;
using System.Windows.Media;
using MegaStarzWP7.ViewModels;
using Megastar.Client.Library;
using Megastar.RestServices.Library.Entities;
using Microsoft.Phone.Controls;


namespace MegaStarzWP7
{
    public partial class KaraokePage : PhoneApplicationPage
    {

        #region Members

        private FileSink sink;
        private CaptureSource captureSource;
        private string m_capturedFileName = "myVideo.mp4";
        
        #endregion

        #region CTOR

        public KaraokePage()
        {
            DataContext = ((App) Application.Current).SongList;
            InitializeComponent();
        }

        #endregion

        private void OnFuncButtonClick(object sender, RoutedEventArgs e)
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
                
                //start capture and show video
                captureSource.Start();
        }

    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
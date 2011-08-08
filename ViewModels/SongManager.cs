using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Megastar.RestServices.Library.Entities;

namespace MegaStarzWP7.ViewModels
{
    public class SongManager
    {
        public readonly static string fileDirectory = @"Megastarz\Video\"; //TODO: Get from settings
        private static readonly IsolatedStorageFile _isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();

        public static bool CheckIfSongIsLoaded(int id)
        {
            return
            _isolatedStorageFile.FileExists(fileDirectory + id.ToString());
        }



        public static void DownloadAndSaveSongAsync(SongResponse song, Action<SongResponse> callback)
        {
            WebClient webClient = new WebClient();
            
            //webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);

            webClient.OpenReadCompleted += (sender, args) =>
                                               {
                                                   webClient_OpenReadCompleted(sender, args, callback, song);
                                               };
            
            
            webClient.OpenReadAsync(new Uri(song.playUrl));
        }

        private static bool IncreaseIsolatedStorageSpace(long quotaSizeDemand)
        {
            bool CanSizeIncrease = false;
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            //Get the Available space
            long maxAvailableSpace = isolatedStorageFile.AvailableFreeSpace;
            if (quotaSizeDemand > maxAvailableSpace)
            {
                if (!isolatedStorageFile.IncreaseQuotaTo(isolatedStorageFile.Quota + quotaSizeDemand))
                {
                    CanSizeIncrease = false;
                    return CanSizeIncrease;
                }
                CanSizeIncrease = true;
                return CanSizeIncrease;
            }
            return CanSizeIncrease;
        }

        private static void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e, Action<SongResponse> callback, SongResponse song)
        {
            try
            {
                if (e.Result != null)
                {
                    IsolatedStorageFileStream isolatedStorageFileStream;
                    IsolatedStorageFile isolatedStorageFile;

                    #region Isolated Storage Copy Code
                    isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();

                    bool checkQuotaIncrease = IncreaseIsolatedStorageSpace(e.Result.Length);

                    if (!isolatedStorageFile.DirectoryExists("DownloadedSongs"))
                    {
                        isolatedStorageFile.CreateDirectory("DownloadedSongs");
                    }

                    string VideoFile = "DownloadedSongs\\" + song.id.ToString();
                    isolatedStorageFileStream = new IsolatedStorageFileStream(VideoFile, FileMode.Create, isolatedStorageFile);
                    
                    long VideoFileLength = (long)e.Result.Length;
                    byte[] byteImage = new byte[VideoFileLength];
                    
                    e.Result.Read(byteImage, 0, byteImage.Length);
                    isolatedStorageFileStream.Write(byteImage, 0, byteImage.Length);

                    isolatedStorageFileStream.Close();
                    #endregion
                    
                    callback(song);
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}

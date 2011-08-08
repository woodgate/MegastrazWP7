using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MegaStarzWP7.Models;
using Megastar.RestServices.Library.Entities;

namespace MegaStarzWP7.ViewModels
{
    public class SongManager
    {
        public readonly static string fileDirectory = @"Megastarz\Video"; //TODO: Get from settings
        
        public static bool CheckIfSongIsLoaded(int id)
        {
            var _isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            return
            _isolatedStorageFile.FileExists(fileDirectory + "\\" + id.ToString());
        }



        public static void DownloadAndSaveSongAsync(SongModel song)
        {
            if (song.IsLoaded)
                return;

            WebClient webClient = new WebClient();
            
            //webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);


            //Set Callback action on Read Complete
            webClient.OpenReadCompleted += (sender, args) =>
                                               {
                                                   webClient_OpenReadCompleted(sender, args, song);
                                               };
            
            //Start Read Async
            webClient.OpenReadAsync(song.ServerURI);
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

        private static void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e, SongModel song)
        {
            try
            {
                if (e.Result != null)
                {
                    IsolatedStorageFileStream isolatedStorageFileStream;
                    IsolatedStorageFile isolatedStorageFile;

                    //TODO: Check that file save os done correctly
                    #region Isolated Storage Copy Code
                    isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();

                    bool checkQuotaIncrease = IncreaseIsolatedStorageSpace(e.Result.Length);

                    if (!isolatedStorageFile.DirectoryExists(fileDirectory))
                    {
                        isolatedStorageFile.CreateDirectory(fileDirectory);
                    }

                    string VideoFile = fileDirectory + "\\" + song.Id.ToString();
                    isolatedStorageFileStream = new IsolatedStorageFileStream(VideoFile, FileMode.Create, isolatedStorageFile);
                    
                    long VideoFileLength = (long)e.Result.Length;
                    byte[] byteImage = new byte[VideoFileLength];
                    
                    e.Result.Read(byteImage, 0, byteImage.Length);
                    isolatedStorageFileStream.Write(byteImage, 0, byteImage.Length);

                    isolatedStorageFileStream.Close();
                    #endregion
                    
                    //Notify song is loaded
                    song.IsLoaded = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}

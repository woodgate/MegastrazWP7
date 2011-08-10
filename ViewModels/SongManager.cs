using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using MegaStarzWP7.Models;

namespace MegaStarzWP7.ViewModels
{
    /// <summary>
    /// Utility methods library for handling downloading and uploading songs
    /// </summary>
    public class SongManager
    {
        public readonly static string fileDirectory = @"Megastarz"; //TODO: Get from settings
        
        /// <summary>
        /// Checks if the song already on the device
        /// </summary>
        /// <param name="id">the song ID</param>
        /// <returns></returns>
        public static bool CheckIfSongIsLoaded(int id)
        {
            var _isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            return
            _isolatedStorageFile.FileExists(fileDirectory + "\\" + id.ToString() + ".wmv");
        }


        /// <summary>
        /// Download song into the device a-synchronicly
        /// </summary>
        /// <param name="song"></param>
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

                    bool checkQuotaIncrease = FilesManager.CanIsolatedStorageSpaceSizeIncrease(e.Result.Length);

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

        

        public static void InitIsolatedStore()
        {

            FilesManager.CreateDirectory(fileDirectory);

            //Add two song files to the Isolated store
            FilesManager.CopyFileFromXAP("Videos\\Abba_Mamma_Mia.wmv", fileDirectory + "\\" + "3.wmv");

            //Add two song files to the Isolated store
            FilesManager.CopyFileFromXAP("Videos\\Abba_The_Winner_Takes_it_all.wmv", fileDirectory + "\\" + "4.wmv");

        }
    }
}

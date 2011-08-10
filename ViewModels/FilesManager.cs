using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Media;

namespace MegaStarzWP7.ViewModels
{
    /// <summary>
    /// Files utility methods library
    /// </summary>
    public class FilesManager
    {
        
        /// <summary>
        /// Check if there is enough space in the IsolatedStorage
        /// </summary>
        /// <param name="quotaSizeDemand">size of the stored file</param>
        /// <returns>true iff there is enough space</returns>
        public static bool CanIsolatedStorageSpaceSizeIncrease(long quotaSizeDemand)
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();

            //Get the Available space
            long maxAvailableSpace = isolatedStorageFile.AvailableFreeSpace;
            if (quotaSizeDemand > maxAvailableSpace)
            {
                if (!isolatedStorageFile.IncreaseQuotaTo(isolatedStorageFile.Quota + quotaSizeDemand))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Create a new directory
        /// </summary>
        /// <param name="fileDirectory"></param>
        public static void CreateDirectory(string fileDirectory)
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();

            if (!isolatedStorageFile.DirectoryExists(fileDirectory))
            {
                isolatedStorageFile.CreateDirectory(fileDirectory);
            }
        }


        /// <summary>
        /// Copy file from the XAP to the isolated storage
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destinationFileName"></param>
        public static void CopyFileFromXAP(string sourceFileName, string destinationFileName)
        {
            //TODO: 10/8 Handle Errors

            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            
            if (!isf.FileExists(destinationFileName))
	        {
                
                BinaryReader fileReader = new BinaryReader(Application.GetResourceStream(new Uri(sourceFileName, UriKind.Relative)).Stream);

                //Increase Isolated Storage Quota If needed
                bool checkQuotaIncrease = FilesManager.CanIsolatedStorageSpaceSizeIncrease(fileReader.BaseStream.Length);


	            IsolatedStorageFileStream outFile = new IsolatedStorageFileStream(destinationFileName, FileMode.Create, isf);
                    

                
	            bool eof = false;
	            long fileLength = fileReader.BaseStream.Length;
	            int writeLength = 512;
	            while (!eof)
	            {
	                if (fileLength < 512)
	                {
	                    writeLength = Convert.ToInt32(fileLength);
	                    outFile.Write(fileReader.ReadBytes(writeLength), 0, writeLength);
	                }
	                else
	                {
	                    outFile.Write(fileReader.ReadBytes(writeLength), 0, writeLength);
	                }
	 
	                fileLength = fileLength - 512;
	 
	                if (fileLength <= 0) eof = true;
	            }
	            fileReader.Close();
	            outFile.Close();
	        }
        }

        /// <summary>
        ///  Save video capture to an MP4 file in order to play back at a later time, using FileSink
        /// </summary>
        /// <param name="fileName">The file name (without extention!)</param>
        /// <param name="captureSource">The video capture source</param>
        public static void SaveVideoCaptureFile(string fileName, CaptureSource captureSource)
        {
            var sink = new FileSink();
            sink.CaptureSource = captureSource;
            //TODO: Nitzo, pay attention that the captured file is mp4
            sink.IsolatedStorageFileName = fileName + ".mp4";
        }

        /// <summary>
        /// Return a FileStream of the reqused
        /// </summary>
        /// <param name="fileName">the file name  (Including an extention)</param>
        /// <returns>If the file exists return the requested file otherwise null.</returns>
        public static IsolatedStorageFileStream GetFileStream(string fileName)
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store.FileExists(fileName))
                    {
                        IsolatedStorageFileStream stream = store.OpenFile(fileName, FileMode.Open);
                        return stream;
                    }

                    MessageBox.Show("unable to fine the file: " + fileName);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error while trying to open the file " + fileName + " " + exception.Message);
            }

            return null;
        }
    }
}

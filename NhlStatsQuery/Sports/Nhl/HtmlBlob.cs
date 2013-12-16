using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

using HtmlAgilityPack;
using SportsData.Models;

using System.Runtime.CompilerServices;

namespace SportsData.Nhl
{
    public class HtmlBlob
    {
        #region Properties

        private static string storageConnectionString = null;
        public static string StorageConnectionString
        {
            get
            {
                if (null == HtmlBlob.storageConnectionString)
                {
                    HtmlBlob.storageConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
                }

                return HtmlBlob.storageConnectionString;
            }

            set
            {
                HtmlBlob.storageConnectionString = value;
            }
        }

        private static string containerName = null;
        public static string ContainerName
        {
            get
            {
                if (null == HtmlBlob.containerName)
                {
                    HtmlBlob.containerName = ConfigurationManager.AppSettings["ContainerName"];
                }

                return HtmlBlob.containerName;
            }

            set
            {
                HtmlBlob.containerName = value;
            }
        }

        private static CloudBlobContainer cloudBlobContainer = null;
        public static CloudBlobContainer CloudBlobContainer
        {
            get
            {
                if (null == HtmlBlob.cloudBlobContainer)
                {
                    //http://sportsdata.blob.core.windows.net/htmlblobs	or http://sportsdata.blob.core.windows.net/htmlblobs-test
                    CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(HtmlBlob.StorageConnectionString);
                    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                    HtmlBlob.cloudBlobContainer = cloudBlobClient.GetContainerReference(HtmlBlob.ContainerName);
                }

                return HtmlBlob.cloudBlobContainer;
            }

            set
            {
                HtmlBlob.cloudBlobContainer = value;
            }
        }

        #endregion

        public static string GetHtmlPage(Uri uri)
        {
            HttpClient httpClient = new HttpClient();
            Task<string> response = httpClient.GetStringAsync(uri);
            string responseString = response.Result;
            return responseString;

        }

        public static string GetHtmlPage(string uri)
        {
            return HtmlBlob.GetHtmlPage(new Uri(uri));
        }

        public static void SaveAsBlob(Uri uri, string html)
        {
            HtmlBlob.SaveAsBlob(uri.RemoveHttpFromUri(), html);
        }

        public static void SaveAsBlob(string blobName, string html)
        {
            CloudBlockBlob cloudBlockBlob = HtmlBlob.CloudBlobContainer.GetBlockBlobReference(blobName);
            cloudBlockBlob.UploadText(html);
        }

        public static string RetrieveBlob(Uri uri)
        {
            return HtmlBlob.RetrieveBlob(uri.RemoveHttpFromUri());
        }

        public static string RetrieveBlob(string blobName)
        {
            //IEnumerable<IListBlobItem> blobs = HtmlBlob.CloudBlobContainer.ListBlobs(null, false);

            CloudBlockBlob cloudBlockBlob = HtmlBlob.CloudBlobContainer.GetBlockBlobReference(blobName);
            string result = cloudBlockBlob.DownloadText();
            return result;
        }

    }
}

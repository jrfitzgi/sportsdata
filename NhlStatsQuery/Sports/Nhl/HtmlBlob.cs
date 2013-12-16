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
        // Start using other prefixes if/when we start storing blobs from other sources
        private const string nhlBlobNamePrefix = "sports/nhl";

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

        public static void GetAndStoreHtmlBlob(Uri uri, HtmlBlobType htmlBlobType)
        {
            string htmlBlob = HtmlBlob.GetHtmlPage(uri);

        }

        public static string GetHtmlPage(Uri uri)
        {
            HttpClient httpClient = new HttpClient();
            Task<string> response = httpClient.GetStringAsync(uri);
            string responseString = response.Result;
            return responseString;

        }

        private static string GetHtmlPage(string uri)
        {
            return HtmlBlob.GetHtmlPage(new Uri(uri));
        }

        public static void SaveAsBlob(HtmlBlobType htmlBlobType, Uri uri, string html)
        {
            HtmlBlob.SaveAsBlob(HtmlBlob.ConstructBlobName(htmlBlobType, uri), html);
        }

        private static void SaveAsBlob(string blobName, string html)
        {
            CloudBlockBlob cloudBlockBlob = HtmlBlob.CloudBlobContainer.GetBlockBlobReference(blobName);
            cloudBlockBlob.UploadText(html);
        }

        public static string RetrieveBlob(HtmlBlobType htmlBlobType, Uri uri)
        {
            return HtmlBlob.RetrieveBlob(HtmlBlob.ConstructBlobName(htmlBlobType, uri));
        }

        private static string RetrieveBlob(string blobName)
        {
            CloudBlockBlob cloudBlockBlob = HtmlBlob.CloudBlobContainer.GetBlockBlobReference(blobName);
            string result = cloudBlockBlob.DownloadText();
            return result;
        }

        public static bool BlobExists(HtmlBlobType htmlBlobType, Uri uri)
        {
            string blobName = HtmlBlob.ConstructBlobName(htmlBlobType, uri);
            CloudBlockBlob cloudBlockBlob = HtmlBlob.CloudBlobContainer.GetBlockBlobReference(blobName);
            return cloudBlockBlob.Exists();
        }

        private static string ConstructBlobName(HtmlBlobType htmlBlobType, Uri uri)
        {
            string blobName = String.Join("/", HtmlBlob.nhlBlobNamePrefix, htmlBlobType.ToString(), uri.RemoveHttpFromUri());
            return blobName;
        }
    }
}

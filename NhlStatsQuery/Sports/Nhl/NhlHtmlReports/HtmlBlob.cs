using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;



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

        public static void GetAndStoreHtmlBlobs(HtmlBlobType htmlBlobType, Dictionary<Uri,string> items, [Optional] bool forceOverwrite)
        {
            foreach (Uri uri in items.Keys)
            {
                HtmlBlob.GetAndStoreHtmlBlob(htmlBlobType, items[uri], uri, forceOverwrite);
            }
        }

        public static void GetAndStoreHtmlBlob(HtmlBlobType htmlBlobType, string htmlBlobId, Uri uri, [Optional] bool forceOverwrite)
        {
            if (HtmlBlob.BlobExists(htmlBlobType, htmlBlobId, uri) && forceOverwrite == false)
            {
                // Blob exists and we don't want to force an overwrite so do nothing
                return;
            }

            string htmlBlob = HtmlBlob.GetHtmlPage(uri);

            if (!String.IsNullOrWhiteSpace(htmlBlob))
            {
                HtmlBlob.SaveBlob(htmlBlobType, htmlBlobId, uri, htmlBlob);
            }

        }

        public static string GetHtmlPage(Uri uri)
        {
            string responseString = null;

            try
            {
                HttpClient httpClient = new HttpClient();
                Task<string> response = httpClient.GetStringAsync(uri);
                responseString = response.Result;
            }
            catch (System.AggregateException e)
            {
                HtmlBlob.PrintGetHtmlPageException(uri, e);

                bool is404Exception = e.InnerExceptions.Where(ie =>
                    ie.GetType() == typeof(HttpRequestException) &&
                    ie.Message.IndexOf("404 (Not Found).", StringComparison.InvariantCultureIgnoreCase) >= 0).Any();

                if (!is404Exception)
                {
                    //throw;
                }
            }
            catch (Exception e)
            {
                HtmlBlob.PrintGetHtmlPageException(uri, e);
                //throw;
            }

            return responseString;
        }

        private static void PrintGetHtmlPageException(Uri uri, Exception e)
        {
            Console.WriteLine(uri.AbsoluteUri);
            Console.WriteLine(e.ToString());
            Console.WriteLine();
        }

        public static void SaveBlob(HtmlBlobType htmlBlobType, string htmlBlobId, Uri uri, string html)
        {
            string blobName = HtmlBlob.ConstructBlobName(htmlBlobType, htmlBlobId, uri);
            CloudBlockBlob cloudBlockBlob = HtmlBlob.CloudBlobContainer.GetBlockBlobReference(blobName);
            cloudBlockBlob.UploadText(html);
        }

        public static string RetrieveBlob(HtmlBlobType htmlBlobType, string htmlBlobId, Uri uri)
        {
            string blobName = HtmlBlob.ConstructBlobName(htmlBlobType, htmlBlobId, uri);
            CloudBlockBlob cloudBlockBlob = HtmlBlob.CloudBlobContainer.GetBlockBlobReference(blobName);
            string result = cloudBlockBlob.DownloadText();
            return result;
        }

        public static bool BlobExists(HtmlBlobType htmlBlobType, string htmlBlobId, Uri uri)
        {
            string blobName = HtmlBlob.ConstructBlobName(htmlBlobType, htmlBlobId, uri);
            CloudBlockBlob cloudBlockBlob = HtmlBlob.CloudBlobContainer.GetBlockBlobReference(blobName);
            return cloudBlockBlob.Exists();
        }

        private static string ConstructBlobName(HtmlBlobType htmlBlobType, string htmlBlobId, Uri uri)
        {
            string blobName = String.Join("/", HtmlBlob.nhlBlobNamePrefix, htmlBlobType.ToString(), htmlBlobId, uri.RemoveHttpFromUri());
            return blobName;
        }
    }
}

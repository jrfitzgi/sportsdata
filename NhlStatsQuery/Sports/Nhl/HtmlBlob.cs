using System;
using System.Collections.Generic;
using System.Configuration;
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

        public static void GetAndStoreHtmlBlobs(HtmlBlobType htmlBlobType, List<Uri> uris, [Optional] bool forceOverwrite)
        {
            foreach (Uri uri in uris)
            {
                HtmlBlob.GetAndStoreHtmlBlob(htmlBlobType, uri, forceOverwrite);
            }
        }

        public static void GetAndStoreHtmlBlob(HtmlBlobType htmlBlobType, Uri uri, [Optional] bool forceOverwrite)
        {
            if (HtmlBlob.BlobExists(htmlBlobType, uri) && forceOverwrite == false)
            {
                // Blob exists and we don't want to force an overwrite so do nothing
                return;
            }

            string htmlBlob = HtmlBlob.GetHtmlPage(uri);

            if (!String.IsNullOrWhiteSpace(htmlBlob))
            {
                HtmlBlob.SaveBlob(htmlBlobType, uri, htmlBlob);
            }

        }

        public static string GetHtmlPage(Uri uri)
        {
            // TODO: handle 404's, eg. 2014 preseason game 103 http://www.nhl.com/scores/htmlreports/20132014/RO010103.HTM

            string responseString = null;

            try
            {
                HttpClient httpClient = new HttpClient();
                Task<string> response = httpClient.GetStringAsync(uri);
                responseString = response.Result;
            }
            catch (System.Net.Http.HttpRequestException e)
            {

            }
            catch (System.AggregateException e)
            {

            }
            catch (Exception e)
            {

            }
            
            return responseString;
        }

        public static void SaveBlob(HtmlBlobType htmlBlobType, Uri uri, string html)
        {
            string blobName = HtmlBlob.ConstructBlobName(htmlBlobType, uri);
            CloudBlockBlob cloudBlockBlob = HtmlBlob.CloudBlobContainer.GetBlockBlobReference(blobName);
            cloudBlockBlob.UploadText(html);
        }

        public static string RetrieveBlob(HtmlBlobType htmlBlobType, Uri uri)
        {
            string blobName = HtmlBlob.ConstructBlobName(htmlBlobType, uri);
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

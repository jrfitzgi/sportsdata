using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace SportsData.Social
{
    public abstract class SocialBaseQuery
    {
        protected string BaseAddress = null;
        protected string PageFormatString = null;

        public List<Snapshot> GetSnapshots<Snapshot,Account>(List<Account> accounts)
            where Snapshot:SocialBaseSnapshot
            where Account:SocialBaseAccount
        {
            List<Snapshot> results = new List<Snapshot>();

            // Store the date so all records are stamped with the same date
            DateTime dateOfSnapshot = DateTime.UtcNow;

            foreach (Account account in accounts)
            {
                Snapshot accountSnapshot = this.GetSnapshot<Snapshot>(account);
                if (null != accountSnapshot)
                {
                    accountSnapshot.DateOfSnapshot = dateOfSnapshot; // overwrite the date
                    results.Add(accountSnapshot);
                }
            }

            return results;
        }

        public virtual Snapshot GetSnapshot<Snapshot>(SocialBaseAccount account)
            where Snapshot : SocialBaseSnapshot
        {
            // Construct the url
            string relativeUrl = String.Format(PageFormatString, account.Id); // Eg. /torontomapleleafs/likes
            Uri url = new Uri(relativeUrl, UriKind.Relative);

            // Make an http request
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.69 Safari/537.36");
            httpClient.BaseAddress = new Uri(BaseAddress);

            Task<string> httpResponseMessage = httpClient.GetStringAsync(url);
            string responseString = httpResponseMessage.Result;

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseString);

            HtmlNode documentNode = document.DocumentNode;

            SocialBaseSnapshot result = this.ParsePage(documentNode, account);

            result.DateOfSnapshot = DateTime.UtcNow;
            return result as Snapshot;
        }

        protected abstract SocialBaseSnapshot ParsePage(HtmlNode documentNode, SocialBaseAccount account);
    }
}

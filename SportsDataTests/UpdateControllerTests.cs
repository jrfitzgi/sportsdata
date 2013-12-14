using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class UpdateControllerTests
    {
        [TestMethod]
        public void UpdateControllerTest()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(0, 5, 0);
            httpClient.BaseAddress = new Uri("http://localhost:51714");

            Uri requestUri = new Uri("update/index", UriKind.Relative);

            FormUrlEncodedContent content = new FormUrlEncodedContent(new []
            {
                new KeyValuePair<string,string>("key","5774B680-047B-47EB-B465-3DBF946C3E7A")
            });


            Task<HttpResponseMessage> response = httpClient.PostAsync(requestUri, content);
            string result = response.Result.Content.ReadAsStringAsync().Result;
        }
    }
}

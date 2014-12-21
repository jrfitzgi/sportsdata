using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace SportsData
{
    public class WebUpdate
    {
		/// <summary>
		/// Wraps an http call to the MVC web app. Use this in other clients to update through the same code path.
		/// </summary>
        public static string Update(string controllerActionName, bool useLocalhost)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(3, 0, 0);

            if (useLocalhost)
            {
                httpClient.BaseAddress = new Uri("http://localhost:51714");
            }
            else
            {
                httpClient.BaseAddress = new Uri("http://nhlstats.azurewebsites.net");
            }

            string relativeUri = String.Format("update/{0}", controllerActionName);
            Uri requestUri = new Uri(relativeUri, UriKind.Relative);

            FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("key","5774B680-047B-47EB-B465-3DBF946C3E7A")
            });

            Task<HttpResponseMessage> response = httpClient.PostAsync(requestUri, content);
            string result = response.Result.Content.ReadAsStringAsync().Result;

            return result;
        }
    }
}

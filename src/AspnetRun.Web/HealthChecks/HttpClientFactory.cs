using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRun.Web.HealthChecks
{
    public class HttpClientFactory
    {
        public virtual async Task<string> GetAsync(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}

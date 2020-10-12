using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AspnetRun.Web.HealthChecks
{
    public class IndexPageHealthCheck : IHealthCheck
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClientFactory _httpClientFactory;

        public IndexPageHealthCheck(IHttpContextAccessor httpContextAccessor, HttpClientFactory httpClienFactory)
        {
            _httpClientFactory = httpClienFactory ?? throw new ArgumentNullException(nameof(httpClienFactory));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string myUrl = request.Scheme + "://" + request.Host.ToString();

            var pageContents = await _httpClientFactory.GetAsync(myUrl);
            
            if (pageContents.Contains("product1"))
            {
                return HealthCheckResult.Healthy("The check indicates a healthy result.");
            }

            return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
        }
    }
}

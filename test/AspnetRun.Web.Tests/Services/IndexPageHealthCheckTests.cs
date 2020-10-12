using AspnetRun.Web.HealthChecks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AspnetRun.Web.Tests.Services
{
    public class IndexPageHealthCheckTests
    {
        [Fact]
        public async Task CheckHealthAsync_ReadDataFromResponse_ReadData()
        {
            //arrange
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new Mock<HttpContext>();
            var httpRequest = new Mock<HttpRequest>();
            var httpClientFactory = new Mock<HttpClientFactory>();
            var hostString = new HostString("google.com");

            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpRequest.Setup(x => x.Scheme).Returns("http - test");
            httpRequest.Setup(x => x.Host).Returns(hostString);
            httpClientFactory.Setup(x => x.GetAsync("http - test://google.com")).ReturnsAsync(string.Empty);

            var target = new IndexPageHealthCheck(httpContextAccessor.Object, httpClientFactory.Object);

            //act
            await target.CheckHealthAsync(null, default);

            //assert
            httpClientFactory.Verify(x => x.GetAsync("http - test://google.com"), Times.Once);
        }
        [Fact]
        public async Task CheckHealthAsync_ReadDataFromResponseIfTrue_Healthy()
        {
            //arrange
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new Mock<HttpContext>();
            var httpRequest = new Mock<HttpRequest>();
            var hostString = new HostString("google.com");
            var httpClientFactory = new Mock<HttpClientFactory>();

            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpRequest.Setup(x => x.Scheme).Returns("http - test");
            httpRequest.Setup(x => x.Host).Returns(hostString);
            httpClientFactory.Setup(x => x.GetAsync("http - test://google.com")).ReturnsAsync("product1");

            var target = new IndexPageHealthCheck(httpContextAccessor.Object, httpClientFactory.Object);
            
            //act
            var result = await target.CheckHealthAsync(null, default);

            //assert
            using (new AssertionScope())
            {
                result.Status.Should().Be(HealthStatus.Healthy);
                result.Description.Should().Be("The check indicates a healthy result.");
            }
        }
        [Fact]
        public async Task CheckHealthAsync_ReadDataFromResponseIfFalse_Healthy()
        {
            //arrange
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new Mock<HttpContext>();
            var httpRequest = new Mock<HttpRequest>();
            var hostString = new HostString("google.com");
            var httpClientFactory = new Mock<HttpClientFactory>();

            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpRequest.Setup(x => x.Scheme).Returns("http - test");
            httpRequest.Setup(x => x.Host).Returns(hostString);
            httpClientFactory.Setup(x => x.GetAsync("http - test://google.com")).ReturnsAsync(String.Empty);

            var target = new IndexPageHealthCheck(httpContextAccessor.Object, httpClientFactory.Object);

            //act
            var result = await target.CheckHealthAsync(null, default);

            //assert
            using (new AssertionScope())
            {
                result.Status.Should().Be(HealthStatus.Unhealthy);
                result.Description.Should().Be("The check indicates an unhealthy result.");
            }
        }

        [Fact]
        public void Constructor_HttpContextAccessorIsNull_ArgumentNullExceptionExpected()
        {
            //arrage
            var httpClienFactory = new Mock<HttpClientFactory>();

            //act
            Func<IndexPageHealthCheck> func = () => new IndexPageHealthCheck(null, httpClienFactory.Object);

            //assert
            using (new AssertionScope())
            {
                func.Should().ThrowExactly<ArgumentNullException>()
                    .And
                    .ParamName.Should().Be("httpContextAccessor");
            }
        }

        [Fact]
        public void Constructor_ClientFactoryIsNull_ArgumentNullExceptionExpected()
        {
            //arrage
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            //act
            Func<IndexPageHealthCheck> func = () => new IndexPageHealthCheck(httpContextAccessor.Object, null);

            //assert
            using (new AssertionScope())
            {
                func.Should().ThrowExactly<ArgumentNullException>()
                    .And
                    .ParamName.Should().Be("httpClienFactory");
            }
        }
    }
}
